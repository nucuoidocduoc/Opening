using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper
{
    public class DeletingUpdate : Updating, IUpdatingData
    {
        public DeletingUpdate(LocalDataModel localDataModel, Guid drawingId, IRepositoryWrapper repository)
                : base(localDataModel, drawingId, repository)
        {
        }

        public void ImplementUpdate()
        {
            ImplementDeleteFromLocal();
            ImplementUpdatePendingDelete();
        }

        private async void ImplementDeleteFromLocal()
        {
            foreach (var opening in _localDataModel.OpeningsDeletedBelowLocal) {
                // tim manager
                var manager = await _repository.ElementManagement.FindByCondition(m => m.Id.Equals(opening.IdManager)).FirstOrDefaultAsync();

                // tim tat cac cac thuc the manager da phan phoi
                var elements = await _repository.Element.FindByCondition(e => e.IdManager.Equals(manager.Id)).ToListAsync();
                foreach (var ele in elements) {
                    // neu o trang thai normal thi moi xu ly
                    if (ele.Status.Equals("Normal")) {
                        // neu gap thang dang local nay tuc la no da bi delete
                        if (ele.Id.Equals(opening.Id)) {
                            ele.Status = "Deleted";
                        }
                        else {
                            ele.Status = "PendingDelete";
                        }

                        _repository.Element.Update(ele);
                    }
                }
                manager.Status = await IsDeletedAll(manager.Id) ? "Deleted" : "Deleting";
                _repository.ElementManagement.Update(manager);
            }
        }

        private async void ImplementUpdatePendingDelete()
        {
            foreach (var opening in _localDataModel.OpeningsPendingDelete) {
                var manager = await _repository.ElementManagement.FindByCondition(m => m.Id.Equals(opening.IdManager)).FirstOrDefaultAsync();
                if (opening.Action.Equals("DeletedLocal")) {
                    manager.Status = await IsDeletedAll(manager.Id) ? "Deleted" : "Deleting";
                    var element = await _repository.Element.FindByCondition(e => e.Id.Equals(opening.Id)).FirstOrDefaultAsync();
                    element.Status = "Deleted";
                    _repository.Element.Update(element);
                    _repository.ElementManagement.Update(manager);
                }
                else {
                    // khong chi k delete ma con chinh sua roi push lai len server
                    // co 2 job, job 1 la tao moi geometry, job2 la thay doi status
                    manager.Status = "Normal";

                    var elements = await _repository.Element.FindByCondition(e => e.IdManager.Equals(manager.Id)).ToListAsync();
                    foreach (var ele in elements) {
                        if (ele.Status.Equals("Deleted")) {
                            ele.Status = "PendingCreate";
                            ele.IdRevitElement = Guid.Empty;
                        }
                        else {
                            ele.Status = "Normal";
                        }
                        _repository.Element.Update(ele);
                    }

                    _repository.ElementManagement.Update(manager);

                    if (opening.Action.Equals("NoDeleteAndModify")) {
                        GeometryVersion geometryVersion = new GeometryVersion() { Id = new Guid(), IdManager = manager.Id, CreatedDate = DateTime.Now };
                        _repository.GeometryVersion.Add(geometryVersion);
                    }
                }
            }
        }

        private async Task<bool> IsDeletedAll(Guid idManager)
        {
            var elements = await _repository.Element.FindByCondition(e => e.IdManager.Equals(idManager)).ToListAsync();
            return !elements.Any(x => !x.Status.Equals("Deleted"));
        }
    }
}
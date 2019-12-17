using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper
{
    public class CreatingUpdate : Updating, IUpdatingData
    {
        public CreatingUpdate(LocalDataModel localDataModel, Guid drawingId, IRepositoryWrapper repository)
            : base(localDataModel, drawingId, repository)
        {
        }

        public void ImplementUpdate()
        {
            ImplementCreateNewElementFromStack();
            ImplementCreatNewElementPending();
            ImplementCreateNewElementFromLocal();
        }

        private void ImplementCreateNewElementFromStack()
        {
            var openingsFromStack = _localDataModel.NewOpeningsFromStack.Where(x => x.Action.Equals("CreateFromStack"));
            foreach (var opening in openingsFromStack) {
                // create element
                Element element = new Element() { Id = new Guid(), IdManager = opening.IdManager, IdDrawing = _drawingId, IdRevitElement = opening.IdRevitElement, Status = "Normal" };
                _repository.Element.Add(element);
            }
        }

        private void ImplementCreateNewElementFromLocal()
        {
            var openingFromLocal = _localDataModel.NewOpeningsBelowLocal.Where(x => x.Action.Equals("CreateFromLocal"));
            foreach (var opening in openingFromLocal) {
                // create manager
                ElementManagement management = new ElementManagement() { Id = new Guid(), CreatedDate = DateTime.Now, Status = "Normal" };
                _repository.ElementManagement.Add(management);

                //create geometryversion
                GeometryVersion geometryVersion = new GeometryVersion() {
                    Id = new Guid(),
                    IdManager = management.Id,
                    Geometry = opening.Geometry.Geometry,
                    Direction = opening.Geometry.Direction,
                    Original = opening.Geometry.Original,
                    Version = opening.Geometry.Version,
                    CreatedDate = management.CreatedDate,
                    Status = "Normal"
                };
                _repository.GeometryVersion.Add(geometryVersion);

                // create element
                Element element = new Element() { Id = new Guid(), IdManager = management.Id, IdDrawing = _drawingId, IdRevitElement = opening.IdRevitElement, Status = "Normal" };
                _repository.Element.Add(element);
            }
        }

        private async void ImplementCreatNewElementPending()
        {
            var openingReCreate = _localDataModel.OpeningFromPendingCreate.Where(x => x.Action.Equals("ReCreateWhenDeletedChangeToPendingCreate"));
            foreach (var opening in openingReCreate) {
                var element = await _repository.Element.FindByCondition(x => x.Id.Equals(opening.Id)).FirstOrDefaultAsync();
                element.Status = "Normal";
                element.IdRevitElement = opening.IdRevitElement;
                _repository.Element.Update(element);
            }
        }
    }
}
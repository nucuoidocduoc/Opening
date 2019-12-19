using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper.Cluster
{
    public static class UpdateProcessing
    {
        public static void CreateElement(ElementGetDTO elementGetDTO, IRepositoryWrapper repository, Guid idDrawing)
        {
            // can tao manager truoc
            ElementManagement elementManagement = new ElementManagement() {
                Id = new Guid(),
                CreatedDate = DateTime.Now,
                Status = Define.NORMAL
            };

            // Tao geometry

            GeometryVersion geometryVersion = new GeometryVersion() {
                Id = new Guid(),
                IdManager = elementManagement.Id,
                Geometry = elementGetDTO.Geometry.Geometry,
                Direction = elementGetDTO.Geometry.Direction,
                Original = elementGetDTO.Geometry.Original,
                Version = string.Empty,
                CreatedDate = elementManagement.CreatedDate,
                Status = string.Empty,
                IdUserEdited = Guid.Empty
            };

            // Tao Element

            Element element = new Element() {
                Id = new Guid(),
                IdManager = elementManagement.Id,
                IdDrawing = idDrawing,
                IdRevitElement = elementGetDTO.IdRevitElement,
                Status = Define.NORMAL
            };

            repository.ElementManagement.Add(elementManagement);
            repository.Element.Add(element);
            repository.GeometryVersion.Add(geometryVersion);
        }

        public static void CreateElementFromStack(ElementGetDTO elementGetDTO, IRepositoryWrapper repository, Guid idDrawing)
        {
            // Tao Element

            Element element = new Element() {
                Id = new Guid(),
                IdManager = elementGetDTO.IdManager,
                IdDrawing = idDrawing,
                IdRevitElement = elementGetDTO.IdRevitElement,
                Status = Define.NORMAL
            };
        }

        public async static void UpdateElementStatusWithRecreateLocalAsync(ElementGetDTO elementGetDTO, IRepositoryWrapper repository, string status)
        {
            var element = await repository.Element.FindByCondition(x => x.Id.Equals(elementGetDTO.Id)).FirstOrDefaultAsync();
            element.Status = status;
            element.IdRevitElement = elementGetDTO.IdRevitElement;
            repository.Element.Update(element);
        }

        public async static void UpdateElementStatusWithDeletedLocalAndServerWithAsync(ElementGetDTO elementGetDTO, IRepositoryWrapper repository, string status)
        {
            var element = await repository.Element.FindByCondition(x => x.Id.Equals(elementGetDTO.Id)).FirstOrDefaultAsync();
            element.Status = status;
            repository.Element.Update(element);

            // tim manager
            var manager = await repository.ElementManagement.FindByCondition(m => m.Id.Equals(elementGetDTO.IdManager)).FirstOrDefaultAsync();
            manager.Status = await IsDeletedAll(manager.Id, repository) ? Define.DELETED : Define.DELETING;
            repository.ElementManagement.Update(manager);
        }

        public static async void UpdateElementDeletedLocalToServerAsync(ElementGetDTO elementGetDTO, IRepositoryWrapper repository)
        {
            // tim manager
            var manager = await repository.ElementManagement.FindByCondition(m => m.Id.Equals(elementGetDTO.IdManager)).FirstOrDefaultAsync();

            // tim tat cac cac thuc the manager da phan phoi
            var elements = await repository.Element.FindByCondition(e => e.IdManager.Equals(manager.Id)).ToListAsync();
            foreach (var ele in elements) {
                if (ele.Id.Equals(elementGetDTO.Id)) {
                    ele.Status = Define.DELETED;
                    ele.IdRevitElement = Guid.Empty;
                    continue;
                }
                // neu o trang thai normal thi moi xu ly
                if (ele.Status.Equals(Define.NORMAL)) {
                    // neu gap thang dang local nay tuc la no da bi delete

                    ele.Status = Define.PENDING_DELETE;
                }
                else if (ele.Status.Equals(Define.PENDING_CREATE)) {
                    ele.Status = Define.PENDING_DELETE;
                }
                repository.Element.Update(ele);
            }
            manager.Status = await IsDeletedAll(manager.Id, repository) ? Define.DELETED : Define.DELETING;
            repository.ElementManagement.Update(manager);
        }

        public static void UpdateElementGeometryLocalToServer(ElementGetDTO elementGetDTO, IRepositoryWrapper repository)
        {
            // Tao geometry

            GeometryVersion geometryVersion = new GeometryVersion() {
                Id = new Guid(),
                IdManager = elementGetDTO.IdManager,
                Geometry = elementGetDTO.Geometry.Geometry,
                Direction = elementGetDTO.Geometry.Direction,
                Original = elementGetDTO.Geometry.Original,
                Version = string.Empty,
                CreatedDate = DateTime.Now,
                Status = string.Empty,
                IdUserEdited = Guid.Empty
            };
            repository.GeometryVersion.Add(geometryVersion);
        }

        public async static void UpdateElementRecreateOnServerAsync(ElementGetDTO elementGetDTO, IRepositoryWrapper repository)
        {
            // tim manager
            var manager = await repository.ElementManagement.FindByCondition(m => m.Id.Equals(elementGetDTO.IdManager)).FirstOrDefaultAsync();

            // tim tat cac cac thuc the manager da phan phoi
            var elements = await repository.Element.FindByCondition(e => e.IdManager.Equals(manager.Id)).ToListAsync();
            foreach (var ele in elements) {
                if (ele.Id.Equals(elementGetDTO.Id)) {
                    ele.Status = Define.NORMAL;
                    ele.IdRevitElement = elementGetDTO.IdRevitElement;
                    continue;
                }
                // neu o trang thai normal thi moi xu ly
                if (ele.Status.Equals(Define.PENDING_DELETE)) {
                    // neu gap thang dang local nay tuc la no da bi delete

                    ele.Status = Define.NORMAL;
                }
                else if (ele.Status.Equals(Define.DELETED)) {
                    ele.Status = Define.PENDING_CREATE;
                }
                repository.Element.Update(ele);
            }
            manager.Status = Define.NORMAL;
            repository.ElementManagement.Update(manager);

            if (elementGetDTO.DifferenceGeometry) {
                UpdateElementGeometryLocalToServer(elementGetDTO, repository);
            }
        }

        public static async Task<bool> IsDeletedAll(Guid idManager, IRepositoryWrapper repository)
        {
            var elements = await repository.Element.FindByCondition(e => e.IdManager.Equals(idManager)).ToListAsync();
            return !elements.Any(x => !x.Status.Equals(Define.DELETED));
        }
    }
}
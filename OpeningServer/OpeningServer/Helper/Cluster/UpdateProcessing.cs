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
        /// <summary>
        /// Use when one element be created on local and push it to server
        /// Create manager
        /// Create Element with idManager is id of manager just create above, idRevit is id of revit element below local
        /// Create geometry with idManager is id of manager just create above
        /// </summary>
        /// <param name="elementGetDTO"></param>
        /// <param name="repository"></param>
        /// <param name="idDrawing"></param>
        public static void CreateElement(ElementGetDTO elementGetDTO, IRepositoryWrapper repository, Guid idDrawing)
        {
            // can tao manager truoc
            ElementManagement elementManagement = new ElementManagement() {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Status = Define.NORMAL
            };

            // Tao geometry

            GeometryVersion geometryVersion = new GeometryVersion() {
                Id = Guid.NewGuid(),
                IdManager = elementManagement.Id,
                Geometry = elementGetDTO.Geometry.Geometry,
                Direction = elementGetDTO.Geometry.Direction,
                Original = elementGetDTO.Geometry.Original,
                Version = string.Empty,
                CreatedDate = elementManagement.CreatedDate,
                Status = string.Empty,
                //IdUserEdited = Guid.Empty
            };

            // Tao Element

            Element element = new Element() {
                Id = Guid.NewGuid(),
                IdManager = elementManagement.Id,
                IdDrawing = idDrawing,
                IdRevitElement = elementGetDTO.IdRevitElement,
                Status = Define.NORMAL
            };

            repository.ElementManagement.Add(elementManagement);
            repository.Element.Add(element);
            repository.GeometryVersion.Add(geometryVersion);
        }

        /// <summary>
        /// use when add a element no exist on current drawing but exist on other drawing to this drawing
        /// Create element and set idManager=manager, idRevit=id of revit below local
        /// </summary>
        /// <param name="elementGetDTO"></param>
        /// <param name="repository"></param>
        /// <param name="idDrawing"></param>
        public static void CreateElementFromStack(ElementGetDTO elementGetDTO, IRepositoryWrapper repository, Guid idDrawing)
        {
            // Tao Element

            Element element = new Element() {
                Id = Guid.NewGuid(),
                IdManager = elementGetDTO.IdManager,
                IdDrawing = idDrawing,
                IdRevitElement = elementGetDTO.IdRevitElement,
                Status = Define.NORMAL
            };
            repository.Element.Add(element);
        }

        /// <summary>
        /// Use when status on server is pending create or normal and below local we regenerate element
        /// </summary>
        /// <param name="elementGetDTO"></param>
        /// <param name="repository"></param>
        public async static Task<bool> ReGenerateElementBelowLocalAsync(ElementGetDTO elementGetDTO, IRepositoryWrapper repository)
        {
            var element = await repository.Element.FindByCondition(x => x.Id.Equals(elementGetDTO.Id)).FirstOrDefaultAsync();
            element.Status = Define.NORMAL;
            element.IdRevitElement = elementGetDTO.IdRevitElement;
            repository.Element.Update(element);
            return true;
        }

        /// <summary>
        /// Use when status of element of current drawing is pending delete change to deleted
        /// Change status of element to Deleted
        /// Change status of manager to Deleted if all status of other drawing is deleted.
        /// </summary>
        /// <param name="elementGetDTO"></param>
        /// <param name="repository"></param>
        /// <param name="status"></param>
        public async static Task<bool> StatusChangeFromPendingDeleteToDeletedAsync(ElementGetDTO elementGetDTO, IRepositoryWrapper repository)
        {
            var element = await repository.Element.FindByCondition(x => x.Id.Equals(elementGetDTO.Id)).FirstOrDefaultAsync();
            element.IdRevitElement = Guid.Empty;
            element.Status = Define.DELETED;
            repository.Element.Update(element);

            // tim manager
            var manager = await repository.ElementManagement.FindByCondition(m => m.Id.Equals(elementGetDTO.IdManager)).FirstOrDefaultAsync();
            manager.Status = await IsDeletedAll(manager.Id, repository) ? Define.DELETED : Define.DELETING;
            repository.ElementManagement.Update(manager);
            return true;
        }

        /// <summary>
        /// Logic up date when Status of element on server is changed to deleted
        /// Change status of element of other drawing to : Normal->PendingDelete, PendingCreate->Deleted
        /// Change status of element of current drawing to: Deleted
        /// Change IdRevitElement to Guid.empty
        /// Change Status of manager to Deleting or Deleted (if all status of elements are deleted)
        /// </summary>
        /// <param name="elementGetDTO"></param>
        /// <param name="repository"></param>
        public static async Task<bool> UpdateWhenStatusOfElementChangeToDeletedAsync(ElementGetDTO elementGetDTO, IRepositoryWrapper repository)
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
                    ele.Status = Define.DELETED;
                }
                repository.Element.Update(ele);
            }
            manager.Status = await IsDeletedAll(manager.Id, repository) ? Define.DELETED : Define.DELETING;
            repository.ElementManagement.Update(manager);
            return true;
        }

        /// <summary>
        /// Create new geometry to do perform geometry be edited below local and push to server.
        /// </summary>
        /// <param name="elementGetDTO"></param>
        /// <param name="repository"></param>
        public static void CreateNewGeometryVersion(ElementGetDTO elementGetDTO, IRepositoryWrapper repository)
        {
            // Tao geometry

            GeometryVersion geometryVersion = new GeometryVersion() {
                Id = Guid.NewGuid(),
                IdManager = elementGetDTO.IdManager,
                Geometry = elementGetDTO.Geometry.Geometry,
                Direction = elementGetDTO.Geometry.Direction,
                Original = elementGetDTO.Geometry.Original,
                Version = string.Empty,
                CreatedDate = DateTime.Now,
                Status = string.Empty,
                //IdUserEdited = Guid.Empty
            };
            repository.GeometryVersion.Add(geometryVersion);
        }

        /// <summary>
        /// Use when status of element of current drawing on server change from pending delete to normal
        /// Change status of element of other drawing : PendingDelete->Normal, Deleted->PendingCreate
        /// Change status of element of current drawing to: Normal
        /// Change IdRevitElement to id of revit element below local
        /// Change Status of manager to normal
        /// </summary>
        /// <param name="elementGetDTO"></param>
        /// <param name="repository"></param>
        public async static Task<bool> StatusChangeFromPendingDeleteToNormalAsync(ElementGetDTO elementGetDTO, IRepositoryWrapper repository)
        {
            // tim manager
            var manager = await repository.ElementManagement.FindByCondition(m => m.Id.Equals(elementGetDTO.IdManager)).FirstOrDefaultAsync();
            manager.Status = Define.NORMAL;
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

            repository.ElementManagement.Update(manager);

            if (elementGetDTO.DifferenceGeometry) {
                CreateNewGeometryVersion(elementGetDTO, repository);
            }
            return true;
        }

        public async static Task<bool> CreateRevisionAsync(IRepositoryWrapper repository, Guid idDrawing)
        {
            Revision revision = new Revision() { Id = Guid.NewGuid(), IdDrawing = idDrawing, CreatedDate = DateTime.Now };
            repository.Revision.Add(revision);
            var elechecks = repository.Element.FindAll().ToList();
            var manages = repository.ElementManagement.FindAll().ToList();
            var elementsInDrawing = await repository.Element.FindByCondition(e => e.IdDrawing.Equals(idDrawing) &&
            (e.Status.Equals(Define.NORMAL) || e.Status.Equals(Define.PENDING_CREATE)))
            .Include(m => m.ElementManagement)
            .ThenInclude(x => x.GeometryVersions).ToListAsync();

            foreach (var ele in elementsInDrawing) {
                CheckoutVersion checkoutVersion = new CheckoutVersion() {
                    Id = Guid.NewGuid(),
                    IdGeometryVersion = ele.ElementManagement.GeometryVersions.FirstOrDefault().Id,
                    IdRevision = revision.Id
                };
                repository.CheckoutVersion.Add(checkoutVersion);
            }
            return true;
        }

        /// <summary>
        /// User to check all status of elements on drawings are deleted
        /// </summary>
        /// <param name="idManager"></param>
        /// <param name="repository"></param>
        /// <returns>true if all status are deleted</returns>
        public static async Task<bool> IsDeletedAll(Guid idManager, IRepositoryWrapper repository)
        {
            var elements = await repository.Element.FindByCondition(e => e.IdManager.Equals(idManager)).ToListAsync();
            return !elements.Any(x => !x.Status.Equals(Define.DELETED));
        }
    }
}
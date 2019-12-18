using Entities.Models;
using OpeningServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpeningServer.Helper
{
    public static class AutoMapper
    {
        #region mapping to send to local

        public static ElementManagementSendDTO ElementManagerToElementSendDTO(this ElementManagement manager)
        {
            return new ElementManagementSendDTO() {
                Id = manager.Id,
                Geometry = manager.GeometryVersions.FirstOrDefault().GeometryVersionToGeometryDTO(),
                DrawingsContain = manager.Elements.Select(x => x.Drawing.Name).ToList()
            };
        }

        public static GeometryDTO GeometryVersionToGeometryDTO(this GeometryVersion geometry)
        {
            return new GeometryDTO() {
                Geometry = geometry.Geometry,
                Original = geometry.Original,
                Direction = geometry.Direction
            };
        }

        public static ElementSendDTO ElementToElementSendDTO(this Element element)
        {
            return new ElementSendDTO() {
                Id = element.Id,
                IdManager = element.IdManager,
                IdRevitElement = element.IdRevitElement,
                Geometry = element.ElementManagement.GeometryVersions.OrderBy(x => x.CreatedDate).FirstOrDefault().GeometryVersionToGeometryDTO(),
                Status = element.Status
            };
        }

        public static IEnumerable<ElementSendDTO> ConvertElementCollection(this IEnumerable<Element> elements)
        {
            foreach (Element element in elements) {
                yield return element.ElementToElementSendDTO();
            }
        }

        public static IEnumerable<ElementManagementSendDTO> ConvertElementManagementCollection(this IEnumerable<ElementManagement> managements)
        {
            foreach (var manager in managements) {
                yield return manager.ElementManagerToElementSendDTO();
            }
        }

        #endregion mapping to send to local
    }
}
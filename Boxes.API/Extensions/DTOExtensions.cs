﻿using Boxes.API.Application.Commands.BoxContent;
using Boxes.Domain.AggregatesModel.BoxAggregate;

namespace Boxes.API.Extensions
{
    public static class DTOExtensions
    {
        public static IEnumerable<BoxContentDTO> ToBoxContentsDTO(this IEnumerable<BoxContent> boxContents)
        {
            foreach (var content in boxContents)
            {
                yield return content.ToBoxContentDTO();
            }
        }

        public static BoxContentDTO ToBoxContentDTO(this BoxContent content)
        {
            return BoxContentDTO.FromBoxContent(content);
        }

            public static IEnumerable<BoxContent> ToBoxContentsAggregate(this IEnumerable<BoxContentDTO> boxContents)
            {
                foreach (var content in boxContents)
                {
                    yield return content.ToBoxContentAggregate();
                }
            }
            public static BoxContent ToBoxContentAggregate(this BoxContentDTO boxContent)
            {
                return new BoxContent()
                {
                    //Id = boxContent.Id,
                    BoxId = boxContent.BoxId,
                    Description = boxContent.Description,
                    LastKnownBoxId = boxContent.BoxId,
                    Name = boxContent.Name,
                    OrderNumber = 0
                };
            }
       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using static NextGen.BiddingPlatform.Enums.ItemEnums;

namespace NextGen.BiddingPlatform.Enums
{
    public static class Utility
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
        public static List<ListDto> GetProcurementStateList()
        {
            var list = new List<ListDto>();
            foreach (var item in Enum.GetValues(typeof(ProcurementState)).Cast<ProcurementState>())
            {
                list.Add(new ListDto
                {
                    Id = (int)item,
                    Name = item.GetDisplayName()
                });
            }
            return list;
        }
        public static List<ListDto> GetVisibilityList()
        {
            var list = new List<ListDto>();
            foreach (var item in Enum.GetValues(typeof(Visibility)).Cast<Visibility>())
            {
                list.Add(new ListDto
                {
                    Id = (int)item,
                    Name = item.GetDisplayName()
                });
            }
            return list;
        }
        public static List<ListDto> GetItemStatusList()
        {
            var list = new List<ListDto>();
            foreach (var item in Enum.GetValues(typeof(ItemStatus)).Cast<ItemStatus>())
            {
                list.Add(new ListDto
                {
                    Id = (int)item,
                    Name = item.GetDisplayName()
                });
            }
            return list;
        }
    }
    public class ListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

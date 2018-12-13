using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute),
            false);

        if (attributes != null &&
            attributes.Length > 0)
            return attributes[0].Description;
        else
            return value.ToString();
    }

    public static string DisplayName(this Enum value)
    {
        Type enumType = value.GetType();
        var enumValue = Enum.GetName(enumType, value);
        MemberInfo member = enumType.GetMember(enumValue)[0];

        var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
        var outString = ((DisplayAttribute)attrs[0]).Name;

        if (((DisplayAttribute)attrs[0]).ResourceType != null)
        {
            outString = ((DisplayAttribute)attrs[0]).GetName();
        }

        return outString;
    }
}
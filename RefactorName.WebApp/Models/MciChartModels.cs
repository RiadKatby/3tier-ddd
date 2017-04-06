using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.Web.Models
{
    public class MciChart
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public List<MciChartCategory> Data { get; set; }


    }

    public class MciChartCategory
    {
        public string Category { get; set; }

        public List<MciChartValues> Values { get; set; }


    }

    public class MciChartValues
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public bool? IsSliced { get; set; }
        public bool? IsSelected { get; set; }
    }

    public static class ExtensionMethods
    {
        public static List<MciChartCategory> AddMciChartCategory(this MciChart mciChart, string Category)
        {
            if (mciChart.Data == null)
                mciChart.Data = new List<MciChartCategory>();
            mciChart.Data.Add(new MciChartCategory() { Category = Category });
            return mciChart.Data;
        }

        public static List<MciChartCategory> AddMciChartCategory(this List<MciChartCategory> data, string Category)
        {
            data.Add(new MciChartCategory() { Category = Category });
            return data;
        }

        public static List<MciChartValues> AddMciChartValues(this MciChartCategory data, string Name, float Value, bool? IsSliced = null, bool? IsSelected = null)
        {
            if (data.Values == null)
                data.Values = new List<MciChartValues>();
            data.Values.Add(new MciChartValues() { Name = Name, Value = Value, IsSliced = IsSliced, IsSelected = IsSelected });
            return data.Values;
        }

        public static List<MciChartValues> AddMciChartValues(this List<MciChartValues> values, string Name, float Value, bool? IsSliced = null, bool? IsSelected = null)
        {
            values.Add(new MciChartValues() { Name = Name, Value = Value, IsSliced = IsSliced, IsSelected = IsSelected });
            return values;
        }

        public static List<MciChartCategory> AddMciChartCategoryValues(this List<MciChartCategory> data, string Name, float Value, bool? IsSliced = null, bool? IsSelected = null)
        {
            data.Last().AddMciChartValues(Name, Value, IsSliced, IsSelected);
            return data;
        }

        public static string Code(this SaRegions region)
        {
            switch (region)
            {
                case SaRegions.Riyadh:
                    return "sa-ri";
                case SaRegions.Mecca:
                    return "sa-mk";
                case SaRegions.Medinah:
                    return "sa-md";
                case SaRegions.Qassim:
                    return "sa-qs";
                case SaRegions.Sharqiah:
                    return "sa-sh";
                case SaRegions.Aseer:
                    return "sa-as";
                case SaRegions.Hail:
                    return "sa-ha";
                case SaRegions.Tabouk:
                    return "sa-tb";
                case SaRegions.Baha:
                    return "sa-ba";
                case SaRegions.HudodShamaliah:
                    return "sa-hs";
                case SaRegions.Jouf:
                    return "sa-jf";
                case SaRegions.Jeezan:
                    return "sa-jz";
                case SaRegions.Najran:
                    return "sa-nj";
                default:
                    return null;
            }
        }
    }

    public enum SaRegions
    {
        Riyadh = 1,
        Mecca,
        Medinah,
        Qassim,
        Sharqiah,
        Aseer,
        Hail,
        Tabouk,
        Baha,
        HudodShamaliah,
        Jouf,
        Jeezan,
        Najran
    }

    public class MciChartOptions
    {
        // General Settings
        /// <summary>
        /// Sets array of chart's colors    
        /// 
        /// Ex: "red,#000,blue,yellow"
        /// </summary>
        public string Colors { get; set; }

        /// <summary>
        /// Sets cursor of chart    
        /// 
        /// Ex: "pointer"
        /// </summary>
        public string Cursor { get; set; }

        /// <summary>
        /// Disable\Enable Ainmation in entrance   
        /// </summary>
        public bool? Animation { get; set; }
        public bool? DrillDown { get; set; }

        /// <summary>
        /// Show\Hide Legends  
        /// </summary>
        public bool? ShowInLegend { get; set; }
        /// <summary>
        /// Enable\Disable Point Selection   
        /// </summary>
        public bool? AllowPointSelect { get; set; }

        // Tooltip

        /// <summary>
        /// Show\Hide Tooltip  
        /// </summary>
        public bool? Tooltip_Enabled { get; set; }
        /// <summary>
        /// [Bar]Make Tooltip shared for all serieses  
        /// </summary>
        public bool? Tooltip_Shared { get; set; }
        public bool? Tooltip_FollowPointer { get; set; }
        public bool? Tooltip_FollowTouchMove { get; set; }

        // 3d

        /// <summary>
        /// [Pie]Enable\Disable 3d viewing
        /// </summary>
        public bool? O3d_Enabled { get; set; }
        /// <summary>
        /// [Pie]3d Alpha angle
        /// Ex: 45
        /// </summary>
        public int? O3d_Alpha { get; set; }
        /// <summary>
        /// [Pie]3d Beta angle
        /// Ex: 10
        /// </summary>
        public int? O3d_Beta { get; set; }

        // Pie

        /// <summary>
        /// [Pie] Donut Chart inner size
        /// Ex: 80
        /// </summary>
        public int? Pie_InnerSize { get; set; }
        /// <summary>
        /// [Pie] 3d Donut Chart depth size
        /// Ex: 20
        /// </summary>
        public int? Pie_Depth { get; set; }
        /// <summary>
        /// [Pie] Start Angle to draw Pie Chart
        /// Ex: 45
        /// </summary>
        public int? Pie_StartAngle { get; set; }
        /// <summary>
        /// [Pie] End Angle to draw Pie Chart
        /// Ex: 180
        /// </summary>
        public int? Pie_EndAngle { get; set; }
        /// <summary>
        /// [Pie] Show Data and Tooltips in Percentage formate
        /// </summary>
        public bool? Pie_ShowInPercentage { get; set; }

        // Bar

        /// <summary>
        /// [Bar] Enable\Disable Crosshair (Hovering whole x-axis serieces)
        /// </summary>
        public bool? Bar_XAxis_Crosshair { get; set; }
        /// <summary>
        /// [Bar] x-axis label angle
        /// Ex: -45
        /// </summary>
        public int? Bar_XAxis_Rotation { get; set; }
        /// <summary>
        /// [Bar] Y-axis Title
        /// </summary>
        public string Bar_YAxis_Title { get; set; }
        /// <summary>
        /// [Bar] Show Y-bar in right
        /// </summary>
        public bool? Bar_YAxis_RTL { get; set; }

        // Map
        /// <summary>
        /// [Map] Show data in bubbles
        /// </summary>
        public bool? Map_Bubble { get; set; }
        /// <summary>
        /// [Map] Hover Color
        /// </summary>
        public string Map_HoverColor { get; set; }
        /// <summary>
        /// [Map] Min data color
        /// </summary>
        public string Map_MinColor { get; set; }
        /// <summary>
        /// [Map] Max data color
        /// </summary>
        public string Map_MaxColor { get; set; }
        /// <summary>
        /// [Map] Enable\Disable Navigation (zooming, tab-hold)
        /// </summary>
        public bool? Map_Navigation { get; set; }
        /// <summary>
        /// [Map] Show\Hide Navigation Buttons
        /// </summary>
        public bool? Map_NavigationButtons { get; set; }
        /// <summary>
        /// [Map] Map Data Code
        /// Ex: "countries/sa/sa-all"
        /// </summary>
        public string Map_Data { get; set; }
        /// <summary>
        /// [Map] Map Data Join-By code
        /// Ex: "hc-key"
        /// </summary>
        public string Map_JoinBy { get; set; }
        /// <summary>
        /// [Map] User defined Data Legends 
        /// Ex: "[{to: 2},{from:2, to: 4 }]"
        /// </summary>
        public string Map_DataClasses { get; set; }
        /// <summary>
        /// [Map] Enable\Disable Auto Legend Calculations 
        /// </summary>
        public bool? Map_DataClassesAuto { get; set; }
        /// <summary>
        /// [Map] Legend Calculations parts
        /// Ex: 4
        /// </summary>
        public int? Map_DataClassesParts { get; set; }

    }

}
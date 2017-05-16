var GeneralSettings = {
    Colors: ['#0f514d', '#008080', '#73afb7', '#efefef', '#CD860D', '#5D6D1E'],
    Cursor: 'pointer',
    Animation: true,
    DrillDown: false,
    ShowInLegend: true,
    AllowPointSelect: true,
    Shadow: false
}

var Options3d = {
    O3d_Enabled: false,
    O3d_Alpha: 45,
    O3d_Beta: 0,
    O3d_Depth: 0,
    O3d_ViewDistance: 0
}

var Tooltip = {
    Tooltip_Enabled: true,
    Tooltip_BackgroundColor: { linearGradient: [0, 0, 0, 60], stops: [[0, '#FFFFFF'], [1, '#E0E0E0']] },
    Tooltip_Shared: true,
    Tooltip_FollowPointer: false,
    Tooltip_FollowTouchMove: false,

    Tooltip_PiePointFormate: '<b>{point.name}</b>: {point.y}',
    Tooltip_PiePointFormateInPercentage: '<b>{point.name}</b>:%{point.percentage:.1f}',
    Tooltip_PieFormatter: function () { return this.y; },
    Tooltip_PieFormatterInPercentage: function () { return this.percentage.toFixed(1) + '%'; },

    Tooltip_BarHeaderFormat: '<span style="font-size:10px">{point.key}</span><table>',
    Tooltip_BarPointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td><td style="padding:0"><b>{point.y}</b></td></tr>',
    Tooltip_BarFooterFormat: '</table>',
   
    Tooltip_MapPosition: null /* function () {return { x: 0, y: 0 };},*/,
    //MapFormatter: function () { var ret = '<b>' + this.point.name + '</b>'; for (var i = 0; i < Serieses.length; i++) { ret += '<br>'; ret += '<span>' + Serieses[i] + ': ' + (i == 0 ? this.point.value : this.point['value' + (i + 1)]) + '</span>'; } return ret; }
}

var PieChartDefaultSettings = {
    Pie_InnerSize: 0,
    Pie_Depth: 45,
    Pie_StartAngle: 0,
    Pie_EndAngle: 360,
    Pie_ShowInPercentage: false,
    Pie_Formate: '<b>{point.name}</b>: {point.y}',
    Pie_FormateInPercentage: '<b>{point.name}</b>: %{point.percentage:.1f}',
};

var BarChartDefaultSettings = {
    Bar_XAxis_Crosshair: true,
    Bar_XAxis_Rotation: 0,
    Bar_YAxis_Title: 'الإجمالي',
    Bar_YAxis_RTL: true
}

var MapDefaultSettings = {
    Map_Bubble: false,
    Map_HoverColor: '#CD860D',
    Map_MinColor: '#E6E7E8',
    Map_MaxColor: '#5D6D1E',
    Map_BubbleColor: '#5D6D1E',


    Map_Navigation: false,
    Map_NavigationButtons: false,
    //NavigationMouseWheelZoom: false,
    //NavigationTouchZoom: false,

    

    Map_DataClasses: null,
    Map_DataClassesAuto: true,
    Map_DataClassesParts: 4,

    //DataClasses: { Parts: 4, Auto: true, DataClasses: null },
    //Navigation: { Enabled: false, EnableButtons: false, EnableMouseWheelZoom: false, EnableTouchZoom: false, },
    //Title: { Align: 'center', Floating: false, Margin: 15, Style: { "color": "#333333", "fontSize": "18px" }, VerticalAlign: "top", X: 0 },
    //SubTitle: { Align: 'center', Floating: false, Margin: 15, Style: { "color": "#555555" }, UseHTML: true, VerticalAlign: "top", X: 0 },
    //PlotOptionsMap: { allAreas: true, BorderColor: "silver", BorderWidth: 1, color: undefined, DashStyle: "Solid", enableMouseTracking: true, nullColor: "#F8F8F8", selected: false, shadow: false, stickyTracking: false, visible: true },
    Map_Data: 'countries/sa/sa-all',
    Map_JoinBy: 'hc-key'
};

var SettingsType = {
    General: 1,
    Tooltip: 2,
    Options3d: 3,
    Pie: 4,
    Bar: 5,
    Map: 6
}

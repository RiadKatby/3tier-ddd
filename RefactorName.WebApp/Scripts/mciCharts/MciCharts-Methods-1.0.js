// MCI CHARTS JS LIBRARY 1.0
// DRAWING METHODS
// MciCharts-1.0.js, MciCharts-Settings-1.0.js
// Depends on JQUERY and HIGHCHARTS.JS

function DrawPieChart(Container, Data, Settings) {
    if (typeof (Container) == 'string') { Container = $('#' + Container); }
    var colors = GetProperty(Settings, 'Colors', SettingsType.General);
    var iSMultyCategory = (Data.Data.length > 1) ? true : false;
    var IsDrillDown = GetProperty(Settings, 'DrillDown', SettingsType.General);

    dataSeries = [];
    if (!iSMultyCategory) {
        var data = []
        for (var i = 0; i < Data.Data[0].Values.length; i++) {
            data.push({ name: Data.Data[0].Values[i].Name, y: Data.Data[0].Values[i].Value, sliced: Data.Data[0].Values[i].IsSliced, selected: Data.Data[0].Values[i].IsSelected });
        }
        dataSeries.push({ name: 'Cat1', data: data, dataLabels: { useHTML: true, style: { left: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black' }, format: (Settings && Settings.Pie_ShowInPercentage) ? (Settings.Pie_ShowInPercentage) ? Settings.Pie_FormateInPercentage : Settings.Pie_Formate : (PieChartDefaultSettings.Pie_ShowInPercentage) ? PieChartDefaultSettings.Pie_FormateInPercentage : PieChartDefaultSettings.Pie_Formate } });
    }
    else {
        var dataCategories = [], dataValues = [], drilldownObject = [], drilldownData = [], drilldownValue = [];
        for (var i = 0; i < Data.Data.length; i++) {
            var drillDataLen = Data.Data[i].Values.length;
            var objectCategory = { name: Data.Data[i].Category, color: colors[i] }; var objectValues = {};
            var sumCat = 0;
            for (var j = 0; j < Data.Data[i].Values.length; j++) {
                var brightness = 0.2 - (j / drillDataLen) / 5;
                sumCat += Data.Data[i].Values[j].Value;
                if (IsDrillDown) {
                    drilldownValue.push(Data.Data[i].Values[j].Name);
                    drilldownValue.push(Data.Data[i].Values[j].Value);
                    drilldownData.push(drilldownValue);
                    drilldownValue = [];
                }
                else {
                    objectValues = { name: Data.Data[i].Values[j].Name, y: Data.Data[i].Values[j].Value, color: Highcharts.Color(colors[i]).brighten(brightness).get() };
                    dataValues.push(objectValues);
                }
            }
            objectCategory["y"] = sumCat;

            if (IsDrillDown) {
                objectCategory["drilldown"] = objectCategory["name"];
                drilldownObject.push({ id: objectCategory["name"], name: objectCategory["name"], data: drilldownData });
                drilldownData = [];
            }
            dataCategories.push(objectCategory);
        }
        if (IsDrillDown) {
            dataSeries.push({ name: 'Cat1', data: dataCategories, size: '60%', dataLabels: { style: { left: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black' }, format: (Settings && (Settings.Pie_ShowInPercentage === true || Settings.Pie_ShowInPercentage === false)) ? (Settings.Pie_ShowInPercentage) ? Settings.Pie_FormateInPercentage : Settings.Pie_Formate : (PieChartDefaultSettings.Pie_ShowInPercentage) ? PieChartDefaultSettings.Pie_FormateInPercentage : PieChartDefaultSettings.Pie_Formate } });
            }
        else {
            dataSeries.push({ name: 'Cat1', data: dataCategories, size: '60%', dataLabels: { style: { left: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black' }, formatter: function () { return this.y > 5 ? this.point.name : null; }, color: 'white', distance: -30 } });
            dataSeries.push({ name: 'Cat2', data: dataValues, size: '80%', innerSize: '60%', dataLabels: { style: { left: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black' }, format: (Settings && Settings.Pie_ShowInPercentage) ? (Settings.Pie_ShowInPercentage) ? Settings.Pie_FormateInPercentage : Settings.Pie_Formate : (PieChartDefaultSettings.Pie_ShowInPercentage) ? PieChartDefaultSettings.Pie_FormateInPercentage : PieChartDefaultSettings.Pie_Formate } });
        }
    }

    var PieChart = $(Container).highcharts({
        chart: {
            type: 'pie',
            plotShadow: false,
            options3d: {
                enabled: GetProperty(Settings, 'O3d_Enabled', SettingsType.Options3d),
                alpha: GetProperty(Settings, 'O3d_Alpha', SettingsType.Options3d),
                beta: GetProperty(Settings, 'O3d_Beta', SettingsType.Options3d),
            }
        },
        title: { useHTML: true, text: '<span>' + Data.Title + '</span>' },
        subtitle: { useHTML: true, text: '<span>' + Data.SubTitle + '</span>' },
        colors: colors,
        series: dataSeries,//[{ name: 'piechart', data: data }],
        drilldown:  { series: IsDrillDown ? drilldownObject : null } ,
        categories: ['cat1', 'cat2'],
        tooltip: {
            useHTML: true,
            enabled: GetProperty(Settings, 'Tooltip_Enabled', SettingsType.Tooltip),
            pointFormat: GetPointFormat(Settings),
            formatter: GetFormatter(Settings),
            borderWidth: 1,
            borderColor: '#AAA',
            borderRadius: 2
        },
        //labels: { useHTML: true, style: { left: '100px', right: '100px', color: 'red' } },
        plotOptions: {
            pie: {
                innerSize: GetProperty(Settings, 'Pie_InnerSize', SettingsType.Pie),
                depth: GetProperty(Settings, 'Pie_Depth', SettingsType.Pie),
                allowPointSelect: GetProperty(Settings, 'AllowPointSelect', SettingsType.General),
                showPercentage: GetProperty(Settings, 'Pie_ShowInPercentage', SettingsType.Pie),
                showInLegend: (iSMultyCategory) ? false : GetProperty(Settings, 'ShowInLegend', SettingsType.General),
                cursor: GetProperty(Settings, 'Cursor', SettingsType.General),
                animation: GetProperty(Settings, 'Animation', SettingsType.General),
                shadow: GetProperty(Settings, 'Shadow', SettingsType.General),
                //dataLabels: {
                //    useHTML: true,
                //    style: { left: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black' },
                //    format: (Settings && Settings.PlotOptionsPie && Settings.PlotOptionsPie.ShowPercentage) ?
                //        (Settings.PlotOptionsPie.ShowPercentage) ? Settings.PlotOptionsPie.FormateInPercentage : Settings.PlotOptionsPie.Formate :
                //        (PieChartDefaultSettings.PlotOptionsPie.ShowPercentage) ? PieChartDefaultSettings.PlotOptionsPie.FormateInPercentage : PieChartDefaultSettings.PlotOptionsPie.Formate
                //},
                startAngle: GetProperty(Settings, 'Pie_StartAngle', SettingsType.Pie),
                endAngle: GetProperty(Settings, 'Pie_EndAngle', SettingsType.Pie)
            }
        }
    });
}

function DrawBarChart(Container, Data, Settings) {
    if (typeof (Container) == 'string') { Container = $('#' + Container); }
    var colors = GetProperty(Settings, 'Colors', SettingsType.General);

    var Categories = []; var Series = []; var names = []; var values = [[]];
    for (var i = 0; i < Data.Data.length; i++) {
        Categories.push(Data.Data[i].Category);
        for (var j = 0; j < Data.Data[i].Values.length; j++) {
            if (i == 0) {
                values[j] = [];
                names.push(Data.Data[i].Values[j].Name);
                values[j].push(Data.Data[i].Values[j].Value)
            }
            else
                values[j].push(Data.Data[i].Values[j].Value);
        }
    }

    for (var i = 0; i < names.length; i++) {
        Series.push({ name: names[i].toString(), data: values[i] });
    }

    $(Container).highcharts({
        chart: { type: 'column' },
        title: { useHTML: true, text: '<span>' + Data.Title + '</span>' },
        subtitle: { useHTML: true, text: '<span>' + Data.SubTitle + '</span>' },
        xAxis: {
            categories: Categories,
            labels: {
                rotation: GetProperty(Settings, 'Bar_XAxis_Rotation', SettingsType.Bar),
            },
            crosshair: GetProperty(Settings, 'Bar_XAxis_Crosshair', SettingsType.Bar),
        },
        yAxis: {
            title: { useHTML: true, text: GetProperty(Settings, 'Bar_YAxis_Title', SettingsType.Bar) },
            opposite: GetProperty(Settings, 'Bar_YAxis_RTL', SettingsType.Bar)
        },

        //labels: {useHTML: true, items: [{ html: '', style: { left: '50px', top: '18px', color: (Highcharts.theme && Highcharts.theme.textColor) || 'black' }}]},
        tooltip: {
            useHTML: true,
            backgroundColor: "rgba(255,255,255,1)",
            //backgroundColor: Tooltip.Tooltip_BackgroundColor,
            enabled: GetProperty(Settings, 'Tooltip_Enabled', SettingsType.Tooltip),
            shared: GetProperty(Settings, 'Tooltip_Shared', SettingsType.Tooltip),
            headerFormat: GetProperty(Settings, 'Tooltip_BarHeaderFormat', SettingsType.Tooltip),
            pointFormat: GetProperty(Settings, 'Tooltip_BarPointFormat', SettingsType.Tooltip),
            footerFormat: GetProperty(Settings, 'Tooltip_BarFooterFormat', SettingsType.Tooltip),
        },
        colors: colors,
        series: Series
    });
}

function DrawMapChart(Container, Data, Settings) {
    if (typeof (Container) == 'string') { Container = $('#' + Container); }
    var JoinBy = GetProperty(Settings, 'Map_JoinBy', SettingsType.Map);
    var MapData = GetProperty(Settings, 'Map_Data', SettingsType.Map);
    var AllowNavigation = GetProperty(Settings, 'Map_Navigation', SettingsType.Map);

    var Serieses = [];
    for (var i = 0; i < Data.Data[0].Values.length; i++) {
        Serieses.push(Data.Data[0].Values[i].Name);
    }

    var values = [];
    var data = [];
    for (var i = 0; i < Data.Data.length; i++) {

        for (var j = 0; j < Data.Data[i].Values.length; j++) {
            values.push(Data.Data[i].Values[j].Value);
        }
        //var obj = { JoinBy.toString() : Data.Data[i].Category.toString(), 'value': values[0] };
        var obj = {};
        obj[JoinBy] = Data.Data[i].Category.toString();
        obj['value'] = values[0];
        for (var k = 1; k < values.length; k++) {
            obj["value" + (k + 1)] = values[k];
        }
        data.push(obj);
        obj = []; values = [];
    }

    var DataClasses = [];
    var dataClassesMax = Math.max.apply(Math, data.map(function (o) { return o.value; }));
    if ((Settings && Settings.Map_DataClassesAuto) || (MapDefaultSettings.Map_DataClassesAuto)) {
        var parts = GetProperty(Settings, 'Map_DataClassesParts', SettingsType.Map);
        var obj = {}; var tick = dataClassesMax / parts; var cur;
        for (var i = 0; i < parts; i++) {
            if (i == 0) {
                obj['from'] = 0; obj['to'] = cur = tick;
            }
            else if (i == parts.length - 1) {
                obj['from'] = cur; obj['to'] = dataClassesMax;
            }
            else {
                obj['from'] = cur; obj['to'] = cur = cur + tick;
            }
            DataClasses.push(obj);
            obj = {};
        }
    }

    var MapSerieses = [];
    var Bubble = GetProperty(Settings, 'Map_Bubble', SettingsType.Map);
    var ColorAxis = {};
    if (Bubble) {
        DataClasses = null; Settings.Map_DataClasses = null;
        for (var i = 0; i < data.length; i++) {
            data[i]["z"] = data[i]["value"]
        }
        var mapgeojson = Highcharts.geojson(Highcharts.maps[MapData]);
        var series = {
            name: 'Regions',
            mapData: mapgeojson,
            color: '#E0E0E0',
            enableMouseTracking: false,
            dataLabels: {
                enabled: true,
                //useHTML: true,
                format: '{point.name}',
                x: 20,
                y: 4,
            }
        }
        MapSerieses.push(series);
        series = {
            type: 'mapbubble',
            mapData: mapgeojson,
            name: Serieses[0],
            joinBy: JoinBy,
            data: data,
            minSize: 10,// Math.min.apply(Math, data.map(function (o) { return o.value; })),
            maxSize: '12%',
            color: GetProperty(Settings, 'Map_BubbleColor', SettingsType.Map),
        }
        MapSerieses.push(series);
        ColorAxis = null;
    }
    else {
        var series = {
            data: data,
            id: "Series1",
            index: 1,
            allowPointSelect: GetProperty(Settings, 'AllowPointSelect', SettingsType.General),
            mapData: Highcharts.maps[MapData],
            joinBy: JoinBy,
            name: Serieses[0],
            //shadow: false,
            states: {
                hover: { color: GetProperty(Settings, 'Map_HoverColor', SettingsType.Map) },
            },
            dataLabels: {
                enabled: true,
                //useHTML: true,
                format: '{point.name}'
            },
        }
        MapSerieses.push(series);
        var DefaultDataClasses = GetProperty(Settings, 'Map_DataClasses', SettingsType.Map);
        ColorAxis = {
            dataClasses: DefaultDataClasses ? DefaultDataClasses : DataClasses, // DataClasses[{ to: 3 }, { from: 3, to: 5 }, { from: 5, to: 7 }, { from: 7, to: 9 }],
            min: Math.min.apply(Math, data.map(function (o) { return o.value; })),
            max: Math.max.apply(Math, data.map(function (o) { return o.value; })),
            minColor: GetProperty(Settings, 'Map_MinColor', SettingsType.Map),
            maxColor: GetProperty(Settings, 'Map_MaxColor', SettingsType.Map),
        };
    }

    var mapChart = $(Container).highcharts('Map', {
        title: { useHTML: true, text: Data.Title },
        subtitle: { useHTML: true, text: Data.SubTitle },

        mapNavigation: {
            enabled: AllowNavigation,
            enableButtons: GetProperty(Settings, 'Map_NavigationButtons', SettingsType.Map),
            enableMouseWheelZoom: AllowNavigation,
            enableTouchZoom: AllowNavigation,
            buttonOptions: { verticalAlign: 'top' }
        },

        colorAxis: ColorAxis,
        tooltip: {
            useHTML: true,
            enabled: GetProperty(Settings, 'Tooltip_Enabled', SettingsType.Tooltip),
            formatter: function () { var ret = '<b>' + this.point.name + '</b>'; for (var i = 0; i < Serieses.length; i++) { ret += '<br>'; ret += '<span>' + Serieses[i] + ': ' + (i == 0 ? (this.point.value) ? this.point.value : 0 : (this.point['value' + (i + 1)]) ? this.point['value' + (i + 1)] : 0) + '</span>'; } return ret; },
            followPointer: GetProperty(Settings, 'Tooltip_FollowPointer', SettingsType.Tooltip),
            followTouchMove: GetProperty(Settings, 'Tooltip_FollowTouchMove', SettingsType.Tooltip),
            positioner: GetProperty(Settings, 'Tooltip_MapPosition', SettingsType.Tooltip),
        },

        //legend: { useHTML: true },
        plotOptions: {
            map: {
                animation: GetProperty(Settings, 'Animation', SettingsType.General),
                cursor: GetProperty(Settings, 'Cursor', SettingsType.General),
                showInLegend: Bubble ? false : GetProperty(Settings, 'ShowInLegend', SettingsType.General),
            },
            mapbubble: {
                showInLegend: false,
                animation: GetProperty(Settings, 'Animation', SettingsType.General),
                cursor: GetProperty(Settings, 'Cursor', SettingsType.General),
            }
        },
        series: MapSerieses
    });
}

//#region Helper Methods
function GetFormatter(Settings) {
    if (Settings && (Settings.Pie_ShowInPercentage === true || Settings.Pie_ShowInPercentage === false)) {
        if (Settings.Pie_ShowInPercentage === true)
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_FormatterInPercentage : Tooltip.Tooltip_PieFormatterInPercentage;
        else
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_Formatter : Tooltip.PieFormatter
    }
    else {
        if (GeneraPieChartDefaultSettings.ShowInPercentage)
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_FormatterInPercentage : Tooltip.Tooltip_PieFormatterInPercentage;
        else
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_Formatter : Tooltip.Tooltip_PieFormatter
    }
}

function GetPointFormat(Settings) {
    if (Settings && (Settings.Pie_ShowInPercentage === true || Settings.Pie_ShowInPercentage === false)) {
        if (Settings.Pie_ShowInPercentage === true)
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_PointFormateInPercentage : Tooltip.Tooltip_PiePointFormateInPercentage;
        else
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_PointFormate : Tooltip.Tooltip_PiePointFormate
    }
    else {
        if (GeneraPieChartDefaultSettings.ShowInPercentage)
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_PointFormateInPercentage : Tooltip.Tooltip_PiePointFormateInPercentage;
        else
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_PointFormate : Tooltip.Tooltip_PiePointFormate
    }
}

function GetLabelFormat(Settings) {
    if (Settings && (Settings.Pie_ShowInPercentage === true || Settings.Pie_ShowInPercentage === false)) {
        if (Settings.Pie_ShowInPercentage === true)
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_FormatterInPercentage : Tooltip.Tooltip_PieFormatterInPercentage;
        else
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_Formatter : Tooltip.Tooltip_PieFormatter
    }
    else {
        if (GeneraPieChartDefaultSettings.ShowInPercentage)
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_FormatterInPercentage : Tooltip.Tooltip_PieFormatterInPercentage;
        else
            return Settings.Tooltip_PointFormateInPercentage ? Settings.Tooltip_Formatter : Tooltip.Tooltip_PieFormatter
    }
}

function GetProperty(Settings, PropertyName, SettingType) {
    if (Settings && (Settings[PropertyName] === true || Settings[PropertyName] === false || Settings[PropertyName]))
        return PropertyName == "Colors" ? Settings[PropertyName].split(',') : Settings[PropertyName];
    else {
        switch (SettingType) {
            case SettingsType.General:
                return GeneralSettings[PropertyName];
            case SettingsType.Tooltip:
                return Tooltip[PropertyName];
            case SettingsType.Options3d:
                return Options3d[PropertyName];
            case SettingsType.Pie:
                return PieChartDefaultSettings[PropertyName];
            case SettingsType.Bar:
                return BarChartDefaultSettings[PropertyName];
            case SettingsType.Map:
                return MapDefaultSettings[PropertyName];
            default:

        }
    }
}
//#endregion





/*jshint undef: false, unused: false, indent: 2*/
/*global angular: false */

'use strict';

var app = angular.module('mciTemplate', ['multi-select-tree']);

app.controller('billingAppCtrl', ['$scope', function ($scope) {

    var data1 = [];

    for (var i = 0; i < 7; i++) {
        var obj = {
            id: i,
            name: 'Node ' + i,
            children: []
        };

        for (var j = 0; j < 3; j++) {
            var obj2 = {
                id: j,
                name: 'Node ' + i + '.' + j,
                children: []
            };
            obj.children.push(obj2);
        }

        data1.push(obj);
    }

    data1[1].children[0].children.push({
        id: j,
        name: 'Node sub_sub 1',
        children: [],
        selected: true
    });

    $scope.data = angular.copy(data1);

    var data3 = [];

    for (var i = 0; i < 7; i++) {
        var obj3 = {
            id: i,
            name: 'Node new view ' + i
        };
        data3.push(obj3);
    }

    //$scope.postData = function () {
    //    //selectedItem2
    //    //console.log($scope.selectedItem2);

    //};

    $scope.itemSelected = function (item) {
        if ($scope.selectedItem2 !== undefined) {
            var len = $scope.selectedItem2.length;
            for (var idx = 0; idx < len; idx++) {
                //console.log($scope.selectedItem2[idx].selected + '  ' + $scope.selectedItem2[idx].id + '  ' + item.Value);
                if ($scope.selectedItem2[idx].selected && $scope.selectedItem2[idx].id == item.Value) {
                    //console.log('true');
                    return true;
                }
            }
        }
        //console.log('false');
        //console.log(item);
        return false;
    };

    //$scope.selectOnly1Or2 = function (item, selectedItems) {
    //    if (selectedItems !== undefined && selectedItems.length >= 20) {
    //        return false;
    //    } else {
    //        return true;
    //    }
    //};

    $scope.switchViewCallback = function (scopeObj) {

        if (scopeObj.switchViewLabel == 'test2') {
            scopeObj.switchViewLabel = 'test1';
            scopeObj.inputModel = data1;
            scopeObj.selectOnlyLeafs = false;
            scopeObj.selectOnlyLeafs = false;
        } else {
            scopeObj.switchViewLabel = 'test2';
            scopeObj.inputModel = data3;
            scopeObj.selectOnlyLeafs = false;
            scopeObj.selectOnlyLeafs = false;
        }
    }
}]);
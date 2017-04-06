angular.module('multi-select-tree').run(['$templateCache', function ($templateCache) {
    'use strict';

    $templateCache.put('src/multi-select-tree.tpl.html',
      "<div class=\"tree-control treeview\">\n" +
      "\n" +
      "    <div class=\"tree-input\" ng-click=\"onControlClicked($event)\">\n" +
      "    <span ng-if=\"selectedItems.length == 0\" class=\"selected-items\">\n" +
      "      <span ng-bind=\"defaultLabel\"></span>\n" +
      "    </span>\n" +
      "    <span ng-if=\"selectedItems.length > 0\" class=\"selected-items\">\n" +
      "      <div ng-repeat=\"selectedItem in selectedItems\" class=\"selected-item\">{{selectedItem.name}} <span class=\"selected-item-close\"\n" +
      "                                                                                  ng-click=\"deselectItem(selectedItem, $event)\"></span></div>\n" +
      "        <span class=\"caret\"></span>\n" +
      "    </span>\n" +
      "    </div>\n" +
      "    <div class=\"tree-view\" ng-show=\"showTree\">\n" +
      "        <div class=\"helper-container\">\n" +
      "             <div class=\"line\" data-ng-if=\"switchView\">\n" +
      "                 <button type=\"button\" ng-click=\"switchCurrentView($event);\" class=\"helper-button\">{{switchViewLabel}}</button>\n" +
      "             </div>\n" +
      //"            <div class=\"line\">\n" +
      //"                <input placeholder=\"بحث...\" type=\"text\" ng-model=\"filterKeyword\" ng-click=\"onFilterClicked($event)\"\n" +
      //"                       class=\"input-filter\">\n" +
      //"                <span class=\"clear-button\" ng-click=\"clearFilter($event)\"><span class=\"item-close\"></span></span>\n" +
      //"            </div>\n" +
      "        </div>\n" +
      "        <ul class=\"tree-container\">\n" +
      "            <tree-item class=\"top-level\" ng-repeat=\"item in inputModel\" item=\"item\" ng-show=\"!item.isFiltered\"\n" +
      "                       use-callback=\"useCallback\" can-select-item=\"canSelectItem\"\n" +
      "                       multi-select=\"multiSelect\" item-selected=\"itemSelected(item)\"\n" +
      "                       on-active-item=\"onActiveItem(item)\" select-only-leafs=\"selectOnlyLeafs\"></tree-item>\n" +
      "        </ul>\n" +
      "    </div>\n" +
      "</div>\n"
    );


    $templateCache.put('src/tree-item.tpl.html',
      "<li data-mode=\"checkbox\" class=\"node active\" ng-class=\"{'collapsed': !item.isExpanded}\"\n" +
      "    ng-click=\"clickSelectItem(item, $event)\"\n" +
	  "	   ng-mouseover=\"onMouseOver(item, $event)\">\n" +
	  "	<label class=\"input-control\">\n" +
	  "		<input type=\"checkbox\" ng-if=\"showCheckbox()\"\n" +
      "            ng-click=\"clickSelectItem(item, $event)\"\n" +
	  "			   ng-mouseover=\"onMouseOver(item, $event)\"\n" +
	  "			   ng-checked=\"item.selected\"/>\n" +
	  "		<span class=\"check\"></span>\n" +
	  "	</label>\n" +
	  "	<span class=\"leaf\">{{item.name}}</span>\n" +
	  "	<span class=\"node-toggle\" ng-if=\"showExpand(item)\" class=\"expand\"\n" +
      "       ng-click=\"onExpandClicked(item, $event)\"\n" +
	  "		  ng-mouseover=\"onMouseOver(item, $event)\">\n" +
	  "	</span>\n" +
      "\n" +
      " <ul ng-repeat=\"child in item.children\" ng-if=\"item.isExpanded\">\n" +
      "   <tree-item item=\"child\" item-selected=\"subItemSelected(item)\" use-callback=\"useCallback\"\n" +
      "              can-select-item=\"canSelectItem\" multi-select=\"multiSelect\"\n" +
      "              on-active-item=\"activeSubItem(item, $event)\"></tree-item>\n" +
      " </ul>\n" +
      "</li>\n"
    );

}]);
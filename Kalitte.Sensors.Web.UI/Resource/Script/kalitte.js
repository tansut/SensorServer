var ajaxStatus = null;

var nVer = navigator.appVersion;
var nAgt = navigator.userAgent;
var browserName = '';
var fullVersion = 0;
var majorVersion = 0;

if ((verOffset = nAgt.indexOf("MSIE")) != -1) {
    browserName = "Microsoft Internet Explorer";
    fullVersion = parseFloat(nAgt.substring(verOffset + 5));
    majorVersion = parseInt('' + fullVersion);
}

else if ((verOffset = nAgt.indexOf("Opera")) != -1) {
    browserName = "Opera";
    fullVersion = parseFloat(nAgt.substring(verOffset + 6));
    majorVersion = parseInt('' + fullVersion);
}

else if ((verOffset = nAgt.indexOf("Firefox")) != -1) {
    browserName = "Firefox";
    fullVersion = parseFloat(nAgt.substring(verOffset + 8));
    majorVersion = parseInt('' + fullVersion);
}

else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) < (verOffset = nAgt.lastIndexOf('/'))) {
    browserName = nAgt.substring(nameOffset, verOffset);
    fullVersion = parseFloat(nAgt.substring(verOffset + 1));
    if (!isNaN(fullVersion)) majorVersion = parseInt('' + fullVersion);
    else { fullVersion = 0; majorVersion = 0; }
}

if (browserName.toLowerCase() == browserName.toUpperCase()
 || fullVersion == 0 || majorVersion == 0) {
    browserName = navigator.appName;
    fullVersion = parseFloat(nVer);
    majorVersion = parseInt(nVer);
}

if (browserName != "Microsoft Internet Explorer") {
    HTMLAnchorElement.prototype.click = function () {
        var evt = window.document.createEvent('MouseEvents');
        evt.initMouseEvent('click', true, true, this.ownerDocument.defaultView, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
        this.dispatchEvent(evt);
    }
}


String.prototype.endsWidth = function (s) {
    return this.length >= s.length && this.substr(this.length - s.length) == s;
}

var renderStateColor = function (value, meta, record) {
    var template = '<span style="color:{0};">{1}</span>';
    return String.format(template, (value == 'Running') ? "green" : "black", value);
}


Ext.net.DirectEvent.override({
    showFailure: function (response, errorMsg) {
        if (Ext.isEmpty(errorMsg))
            errorMsg = response.responseText;
        var userMsg = errorMsg;
        try {
            userMsg = (Ext.decode(errorMsg).message);
        } catch (e) {
        }
        Ext.MessageBox.show({ title: 'Error', msg: userMsg, icon: Ext.MessageBox.ERROR, width: 450, buttons: Ext.MessageBox.OK });
    }
});

var prepareGridCommand = function (grid, toolbar, rowIndex, record) {
    var cmd = toolbar.items.itemAt(0);

    if (record.get("State") == 'Running') {
        cmd.setIconClass('icon-stopblue');
        cmd.command = grid.stopItemCommandName;
        cmd.setTooltip('Stop');
    } else if (record.get('Startup') == 'Disabled') {
        cmd.setDisabled(true);
        cmd.command = '';
        cmd.setTooltip('Item is disabled. Cannot start.');
    }
};

var filterTree = function (el, e) {
    var tree = pageContentHolder_ctlMenuTree,
        text = this.getRawValue();

    if (e.getKey() === 40) {
        tree.getRootNode().select();
    }

    if (Ext.isEmpty(text, false)) {
        clearFilter(el);
    }

    if (text.length < 3) {
        return;
    }

    tree.clearFilter();

    if (Ext.isEmpty(text, false)) {
        return;
    }

    el.triggers[0].show();

    if (e.getKey() === Ext.EventObject.ESC) {
        clearFilter(el);
    } else {
        var re = new RegExp(".*" + text + ".*", "i");

        tree.getRootNode().collapse(true, false);

        tree.filterBy(function (node) {
            var match = re.test(node.text.replace(/<span>&nbsp;<\/span>/g, "")),
                pn = node.parentNode;

            if (match && node.isLeaf()) {
                pn.hasMatchNode = true;
            }

            if (pn != null && pn.fixed) {
                if (node.isLeaf() === false) {
                    node.fixed = true;
                }
                return true;
            }

            if (node.isLeaf() === false) {
                node.fixed = match;
                return match;
            }

            return (pn != null && pn.fixed) || match;
        }, { expandNodes: false });

        tree.getRootNode().cascade(function (node) {
            if (node.isRoot) {
                return;
            }

            if ((node.getDepth() === 1) ||
               (node.getDepth() === 2 && node.hasMatchNode)) {
                node.expand(false, false);
            }

            delete node.fixed;
            delete node.hasMatchNode;
        }, tree);
    }
};

var clearFilter = function (el, trigger, index, e) {
    var tree = pageContentHolder_ctlMenuTree;
    el.setValue("");
    el.triggers[0].hide();
    tree.clearFilter();
    tree.getRootNode().collapseChildNodes(true);
};

function openPageAsTab(url, title) {
    openTab(Ext.getCmp("pageContentHolder_ExampleTabs"), url, title);
}


function openTab(tabPanel, url, title) {
    var tab = tabPanel.getItem(url);
    var doReload = false;
    if (!tab) {

        tab = tabPanel.add({
            border: false,
            id: url,
            title: title,
            closable: !url.endsWidth("/pages/default.aspx"),
            autoLoad: {
                showMask: true,
                url: url,
                mode: 'iframe',
                maskMsg: title + ' is loading ...'
            }
        });
    } else doReload = true;
    tabPanel.setActiveTab(tab);
    if (doReload) tab.reload();
}

function closeTabs(panel) {
    var idList = new Array();
    var defTab = null;
    for (var i = 0; i < panel.items.length; i++) {
        if (panel.items.items[i].closable)
            idList.push(panel.items.items[i].id);
        else defTab = panel.items.items[i];
    }
    for (var i = 0; i < idList.length; i++)
        panel.closeTab(idList[i], "close");
    if (defTab)
        panel.setActiveTab(defTab);
}

var loadPage = function (tabPanel, node) {
    node.select();

    var tab = tabPanel.getItem(node.id);
    var doReload = false;
    if (!tab) {

        tab = tabPanel.add({
            border: false,
            id: node.id,
            title: node.text,
            iconCls: node.ui.iconNode.className,
            closable: !node.id.endsWith("/pages/default.aspx"),
            autoLoad: {
                showMask: true,
                url: node.attributes.href,
                mode: 'iframe',
                maskMsg: ' Loading ' + node.text
            }
        });
    } else doReload = true;
    tabPanel.setActiveTab(tab);
    if (doReload) tab.reload();
}


var refreshTree = function (tree) {
    TT.RefreshMenu({
        success: function (result) {
            var nodes = eval(result);
            if (nodes.length > 0) {
                tree.initChildren(nodes);
            }
            else {
                tree.getRootNode().removeChildren();
            }
            tree.root.expand(true);
        }
    });
}

function changeTheme(item) {
    Ext.net.ResourceMgr.setTheme(item.theme);
    pageContentHolder_ExampleTabs.items.each(function (p) {
        if (!Ext.isEmpty(p.iframe)) {
            if (p.getBody().Ext) {
                p.getBody().Ext.net.ResourceMgr.setTheme(item.theme);
            }
        }
    });
}

function GridCommandHandler(command, record, grid) {
    grid.getSelectionModel().selectById(record.id);
    var cont = true;
    Ext.each(grid.colModel.columns, function (col) {
        if (col.commands) {
            Ext.each(col.commands, function (cmd) {
                if (cmd.command == command && cmd.confirm == true) {
                    if (!confirm("Continue operation ?"))
                        cont = false;
                }
            }
            );
        }
    }
    );
    return cont;
}

function getGridState(id) {
    var grid = Ext.getCmp(id);
    var store = grid.getStore();

    var data = { filterValues: grid.filters.getFilterData(), sort: grid.getState().sort };

    return Ext.encode(data);
}

function getGridSelectedId(grid) {
    if (grid.getSelectionModel().hasSelection()) {
        return grid.getSelectionModel().selections.items[0].id;
    } else return null;
}


var gridRowDblClick = function (el, rowIndex, e) {
    el.fireEvent('command', 'EditInEditor', el.getSelectionModel().selections.items[0], rowIndex, 0);
};

function getGridSelectedId(grid) {
    if (grid.getSelectionModel().hasSelection()) {
        return grid.getSelectionModel().selections.items[0].id;
    } else return null;
}

function propertyEditorWindowLoaded(el) {
    el.body.first().dom.contentWindow.StartEdit(el.editorControl.getValue());
}

function propertyEditorWindow(type, metaDataType, propertyGroup, propertyName, ctrlId, editorUrl) {
    var pageUrl = editorUrl + "?type=" + type + "&metadataType=" + metaDataType + "&propertyGroup=" + propertyGroup + "&propertyName=" + propertyName + "&R=" + Math.random() * 100;

    win = new Ext.Window(
                            {
                                layout: 'fit',
                                width: 500,
                                height: 500,
                                closeAction: 'close',
                                plain: false,
                                modal: true,
                                title: "Edit Property " + propertyName,
                                maximizable: true,
                                autoLoad: {
                                    showMask: true,
                                    url: pageUrl,
                                    mode: 'iframe',
                                    maskMsg: 'Loading ...',
                                    callback: propertyEditorWindowLoaded
                                }

                            });
    win.on("maximize",
                                        function (el) {
                                            var v = Ext.getBody().getViewSize();
                                            el.setSize(v.width, v.height);
                                        });
    win.on("maximize",
                                        function (el) {
                                            var v = Ext.getBody().getViewSize();
                                            el.setSize(v.width, v.height);
                                        });

    win.on("show",
                                        function (el) {

                                        });
    win.on("beforeclose", function (el) {
        var iFrame = win.body.first();
        delete (iFrame);
        window.PropertyEditorWindow = null;
    });
    window.PropertyEditorWindow = win;
    window.PropertyEditorWindow.editorControl = Ext.getCmp(ctrlId);
    window.PropertyEditorWindow.show();
};

var getLogViewerRowClass = function (record) {
    switch (record.data.Level) {
        case 'Error': return "logViewerErrorRowClass"
            break;
        case 'Fatal': return "logViewerFatalRowClass"
            break;
        case 'Warning': return "logViewerWarningRowClass"
            break;
    }

}
var getLastEventViewerRowClass = function (record, index, rowParams, store) {
    var recordDate = new Date(record.data.EventTime);
    var lastDate = store.lastRefreshDate;
    if (recordDate > lastDate) return "lastEventViewerRowClass";
}




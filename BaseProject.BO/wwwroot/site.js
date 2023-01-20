function cleanfilter() {
    let userFilter = $("#UserFilter").data("kendoTextBox");
   
    if (userFilter) {
        userFilter.value("");
    }

    refreshgrid();
}

function refreshgrid(e) {
    if (e) {
        e.sender.read();
    } else {
        refreshmaingrid();
    }
}





function refreshmaingrid() {
    var sgrid = $("#grid").data("kendoGrid");
    if (sgrid) {
        sgrid.dataSource.query({
            page: 1,
            pageSize: sgrid.dataSource.pageSize()
        });
    } else {
        refreshallgrids();
    }

}
function refreshgridid(gridid) {
    var sgrid = $("#" + gridid).data("kendoGrid");
    if (sgrid) {
        sgrid.dataSource.query({
            page: 1,
            pageSize: sgrid.dataSource.pageSize()
        });
    } else {
        refreshallgrids();
    }

}


function refreshallgrids() {
    var sgrids = $(".k-grid")
    if (sgrids.length > 0) {
        for (var i = 0; i < sgrids.length; i++) {
            var sgrid = $("#" + sgrids[i].id).data("kendoGrid");
            if (sgrid) {
                sgrid.dataSource.query({
                    page: 1,
                    pageSize: sgrid.dataSource.pageSize()
                });
            }
        }
    } else {
        location.reload();
    }

}
function resizegrids() {
    var sgrids = $(".k-grid:visible");
    if (sgrids.length > 0) {
        for (var i = 0; i < sgrids.length; i++) {
            var sgrid = $("#" + sgrids[i].id).data("kendoGrid");
            if (sgrid) {
                sgrid.refresh();
            }
        }
    }
}

function returnGridFilter(e) {
    var grid = $("#" + e).data("kendoGrid").dataSource;
    var filter = grid.transport.parameterMap({ filter: grid.filter() });
    return filter.filter;
}



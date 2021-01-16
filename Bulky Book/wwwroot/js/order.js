var dataTable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        LoadDataTable("GetOrderList?status=inprocess");
    }
    else {
        if (url.includes("pending")) {
            LoadDataTable("GetOrderList?status=pending");
        }
        else {
            if (url.includes("completed")) {
                LoadDataTable("GetOrderList?status=completed");
            }
            else {
                if (url.includes("rejected")) {
                    LoadDataTable("GetOrderList?status=rejected");
                }
                else {
                    LoadDataTable("GetOrderList?status=all");
                }
            }
        }
    }
});

function LoadDataTable(url) {
    dataTable = $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/Order/" + url
        },
        "columns": [
            {
                "data": "id",
                "width": "10%"
            },
            {
                "data": "name",
                "width": "15%"
            },
            {
                "data": "phoneNumber",
                "width": "15%"
            },
            {
                "data": "applicationUser.email",
                "width": "15%"
            },
            {
                "data": "orderStatus",
                "width": "15%"
            },
            {
                "data": "orderTotal",
                "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class="btn btn-sm btn-success" href="/Admin/Order/Details/${data}"><i class="fas fa-edit"></i></a>
                        </div>`;
                },
                "width": "5%"
            }
        ]
    });
}

    //No library/plugins version
    /*
    var remove = confirm("You're about to remove the category, this action can't be reverted, are you sure?");
    var params = { id: id };
    console.log(params);
    if (remove) {
        $.ajax({
            url: "/Admin/Category/Remove/" + id,
            method: "post",
            dataType: "json",
            data: params,
            success: function (data) {
                if (data.result == true) {
                    dataTable.ajax.reload();
                }
                else {

                }
            }
        });
    }
    */
var dataTable;
$(document).ready(function () {
    LoadDataTable();
});

function LoadDataTable() {
    dataTable = $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            {
                "data": "name",
                "width": "15%"
            },
            {
                "data": "streetAddress",
                "width":"15%"
            },
            {
                "data": "city",
                "width":"15%"
            },
            {
                "data": "phoneNumber",
                "width": "15%"
            },
            {
                "data": "isAuthorizedCompany",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" checked disabled />`
                    }
                    else {
                        return `<input type="checkbox" disabled/>`
                    }
                },
                "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <button onclick="javascript:Delete(${data})" class="btn btn-sm btn-danger"><i class="fas fa-trash-alt"></i>Delete</button>
                            <a class="btn btn-sm btn-success" href="/Admin/Company/Upsert/${data}"><i class="fas fa-edit"></i>Edit</a>
                        </div>`;
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(id) {
    //SweetAlert version
    swal({
        title: "Are you sure?",
        text: "You're about to remove the company, this action can't be reverted, are you sure?",
        icon: "warning",
        buttons: ["Cancel", "Confirm"],
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            var params = { id: id };
            $.ajax({
                url: "/Admin/Company/Remove/" + id,
                method: "post",
                dataType: "json",
                data: params,
                success: function (data) {
                    if (data.result == true) {
                        swal("The Company has been deleted!", {
                            icon: "success",
                        });
                        dataTable.ajax.reload();
                    }
                    else {
                        swal("The Company couldn't be removed");
                    }
                }
            });
        }
    });

    //No library/plugins version
    /*
    var remove = confirm("You're about to remove the Company, this action can't be reverted, are you sure?");
    var params = { id: id };
    console.log(params);
    if (remove) {
        $.ajax({
            url: "/Admin/Company/Remove/" + id,
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
}
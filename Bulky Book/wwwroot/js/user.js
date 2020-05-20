var dataTable;
$(document).ready(function () {
    LoadDataTable();
});

function LoadDataTable() {
    dataTable = $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            {
                "data": "name",
                "width": "15%"
            }, {
                "data": "email",
                "width": "15%"
            }, {
                "data": "phoneNumber",
                "width": "15%"
            }, {
                "data": "company.name",
                "width": "15%"
            }, {
                "data": "role",
                "width": "15%"
            },
            {
                "data": { id: "id", lockoutEnd: "lockoutEnd"},
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                        <div class="text-center">
                            <a onclick="LockUnlock('${data.id}')" class="btn btn-sm btn-danger"><i class="fas fa-lock-open"></i>Unlock</a>
                        </div>`;
                    }
                    else {
                        return `
                        <div class="text-center">
                            <a onclick="LockUnlock('${data.id}')" class="btn btn-sm btn-success"><i class="fas fa-lock"></i>Lock</a>
                        </div>`;
                    }
                    
                },
                "width": "25%"
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        url: "/Admin/User/LockUnlock",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(id),
        success: function (data) {
            if (data.success == true) {
                swal(data.message, {
                    icon: "success",
                });
                dataTable.ajax.reload();
            }
            else {
                swal(data.message);
            }
        }
    });

    //No library/plugins version
    /*
    var remove = confirm("You're about to remove the User, this action can't be reverted, are you sure?");
    var params = { id: id };
    console.log(params);
    if (remove) {
        $.ajax({
            url: "/Admin/User/Remove/" + id,
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
var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#table-data').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "10%" },
            { "data": "shortDescription", "width": "10%" },
            { "data": "price", "width": "5%" },
            { "data": "manufacturer.name", "width": "10%" },
            { "data": "category.name", "width": "10%"},
            {
                "data": "imageURL",
                "render": function (data) {
                    return `
                            <img src="${data}" class="img-thumbnail"/>
                            `;

                },
                "width": "10%"
            },
            {
                "data": "lastEdited",
                "render": function (data) {
                    var date = new Date(data);
                    var formatDate = date.getDate() + "." + (date.getMonth() + 1) + '.' + date.getFullYear();
                    formatDate += '\n';
                    formatDate += date.getHours() + ":" + date.getMinutes();
                    return `${formatDate}`;
                },
                "width": "5%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/Admin/Product/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; margin-top:20px">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                },
                "width":"5%"
            }
        ]
    });
    $('#table-data').removeClass('dataTable');
}


function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
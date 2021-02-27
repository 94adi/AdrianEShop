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
            { "data": "title", "width": "6%" },
            { "data": "shortDescription", "width": "6%" },
            { "data": "longDescription", "width": "6%" },
            { "data": "price", "width": "6%" },
            { "data": "discountCode", "width": "6%" },
            { "data": "manufacturer.name", "width": "6%" },
            { "data": "category.name", "width": "6%" },
            { "data": "dateOfManufacture", "width": "6%" },
            {
                "data": "imageURL",
                "render": function (data) {
                    return `
                            <img src="${data}" class="img-thumbnail"/>
                            `;

                },
                "width": "6%"
            },
            { "data": "published", "width": "6%" },
            { "data": "lastEdited", "width": "6%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/Admin/Product/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "30%"
            }
        ]
    });
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
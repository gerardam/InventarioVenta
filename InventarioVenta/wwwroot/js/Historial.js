var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "ajax": {
            "url": "/Inventario/Inventario/ObtenerHistorial"
        },
        "columns": [
            {
                "data": "fechaInicial", "width": "15%",
                "render": function (data) {
                    var d = new Date(data);
                    return d.toLocaleString();
                }
            },
            {
                "data": "fechaFinal", "width": "15%",
                "render": function (data) {
                    var d = new Date(data);
                    return d.toLocaleString();
                }
            },
            { "data": "bodega.nombre", "width": "15%" },
            {
                "data": function nombreUsuario(data) {
                    return data.usuarioAplicacion.nombre + " " + data.usuarioAplicacion.apellidos;
                },
                "width": "20%"
            },
            
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Inventario/Inventario/DetalleHistorial/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                Detalle
                            </a>
                        </div>
                        `;
                }, "width": "10%"
            }
        ]
    });
}
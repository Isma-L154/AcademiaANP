function filterTable() {
    var select = document.getElementById("estado");
    var filter = select.value.toUpperCase();
    var table = document.getElementById("solicitudesTable");
    var tr = table.getElementsByTagName("tr");

    for (var i = 1; i < tr.length; i++) {
        var td = tr[i].getElementsByTagName("td")[7]; 
        if (td) {
            var textValue = td.textContent || td.innerText;
            if (filter === "" || textValue.toUpperCase() === filter) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}
function sendDataToServer(CustomerId, data) {

    if (!data) {
        alert("No data found in localStorage.");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/OrderItem/SaveOrder?customerid=" + CustomerId,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (result) {
            if (result.success) {
                alert("Supplier added successfully");
                removeDataFromLocalStorage();
                $("#addEmployeeModal").modal('hide');
            } else {
                alert(result.message);
            }
        },
        error: function () {
            // Handle AJAX error
            alert("An error occurred while processing your request.");
        }
    });
}
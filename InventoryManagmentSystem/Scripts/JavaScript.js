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
            if (result != null) {
                if (result.success) {
                    alert("Orders saved successfully");
                    removeDataFromLocalStorage();
                    var invoiceId = result.invoiceIds; // Replace with the actual invoice ID
                    window.location.href = '/Main/InvoiceView?invoiceIds=' + invoiceId;
                } else {
                    alert("Orders were not saved successfully: " + result.message);
                }
            } else {
                alert("No response data received.");
            }
        },
        error: function () {
            // Handle AJAX error
            alert("An error occurred while processing your request.");
        }
    });
}



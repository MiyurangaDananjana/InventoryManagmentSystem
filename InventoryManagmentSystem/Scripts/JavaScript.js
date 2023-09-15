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
                    // Redirect to the InvoiceView action in the Main controller
                    window.location.href = result.redirectUrl;
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
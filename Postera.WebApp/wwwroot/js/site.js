async function sendRequest(url, method, body) {
    let responseText = await fetch(url,
        {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: body
        }).then(response => response.text());

    return responseText;
}

async function getPostOffices() {
    let tableBody = document.querySelector("#postOfficesList");
    let postOffices = await sendRequest("/postOffices/list");
    tableBody.innerHTML = postOffices;
}

async function getLatestOrders(id, type) {
    let tableBody = document.querySelector("#latestOrders");
    let orders = await sendRequest(`/${type}s/${id}/orders?take=10&orderby.field=SendDate`);
    tableBody.innerHTML = orders;
}

async function getOrders(id, type) {
    let tableBody = document.querySelector("#orders");
    let orders = await sendRequest(`/${type}s/${id}/orders`);
    tableBody.innerHTML = orders;
}



/// modal window

async function registerOrderModal() {
    let table = document.querySelector("#orders");
    table.addEventListener("click",
        async function (event) {
            if (event.target.closest("tr").className == "orderRow") {
                let id = event.target.closest("tr").id;
                await displayOrderModal(id);

                let modal = document.querySelector(".modal");
                modal.className += " show modal-open-scroll";
                modal.style.display = "block";
            }
        });

    let modal = document.querySelector(".modal");
    let closeBtn = document.querySelector(".close");
    closeBtn.addEventListener("click", () => { modal.style.display = "none" });
    window.addEventListener("click", (e) => {
        if (e.target == modal) {
            modal.style.display = "none";
        }
    });
}

async function displayOrderModal(id) {
    let orderSerialized = await sendRequest(`/orders/${id}`);
    let order = JSON.parse(orderSerialized);
    let modal = document.querySelector(".modal");
    var postOfficeSerialized = await sendRequest(`/postOffices/${order.postOfficeId}`);
    var postOffice = JSON.parse(postOfficeSerialized);
    modal.querySelector("#postOfficeCompany").value = postOffice.name;
}
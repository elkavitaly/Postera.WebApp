async function sendRequest(url, method, body, headers) {
    if (!headers) {
        headers = {
            'Content-Type': 'application/x-www-form-urlencoded'
        }
    }

    let responseText = await fetch(url,
        {
            method: method,
            headers: headers,
            body: body
        })
        .then((response) => {
            if (response.status == 500) {
                document.location.replace("/");
            }

            if (response.status == 401) {
                document.location.replace("/user/login");
            }

            return response.text();
        });

    return responseText;
}

async function getItemsList(itemType) {
    let tableBody = document.querySelector(`#itemsList`);
    let items = await sendRequest(`/${itemType}`);
    tableBody.innerHTML = items;
}

async function getLatestOrders(id, type) {
    let tableBody = document.querySelector("#latestOrders");
    let orders = await sendRequest(`/${type}/${id}/orders?take=10&orderby.field=SendDate`);
    tableBody.innerHTML = orders;
}

async function getOrders(id, type) {
    let tableBody = document.querySelector("#orders");
    let orders = await sendRequest(`/${type}/${id}/orders`);
    tableBody.innerHTML = orders;
}

/// modal window

async function registerOrderModal() {
    let table = document.querySelector("#orders");
    table.addEventListener("click",
        async function (event) {
            if (event.target.closest("tr").className == "orderRow") {
                let id = event.target.closest("tr").id;
                displayOrderModal(id);
            }
        },
        false);
}

async function displayOrderModal(id) {
    let modalWindow = await sendRequest(`/orders/${id}/modal`);
    document.querySelector("#modalWindowBlock").innerHTML += modalWindow;

    onModalDisplay();
}

function onModalDisplay() {
    let modal = document.querySelector(".modal");
    modal.className += " show modal-open-scroll";
    modal.style.display = "block";

    let closeBtn = document.querySelector(".close");
    closeBtn.addEventListener("click", onModalClose);
    window.addEventListener("click", (e) => {
        if (e.target == modal) {
            onModalClose();
        }
    });
}

function onModalClose() {
    let modal = document.querySelector(".modal");
    document.querySelector("#modalWindowBlock").removeChild(modal);
}

async function getOrder(id) {
    let orderSerialized = await sendRequest(`/orders/${id}`);
    let order = JSON.parse(orderSerialized);

    return order;
}

async function getPostOffice(id) {
    var postOfficeSerialized = await sendRequest(`/postOffices/${id}`);
    var postOffice = JSON.parse(postOfficeSerialized);

    return postOffice;
}

async function getStorage(id) {
    var storageSerialized = await sendRequest(`/storages/${id}`);
    var storage = JSON.parse(storageSerialized);

    return storage;
}

async function registerAddModal(itemType) {
    let button = document.querySelector("#addItemButton");
    let type = itemType.toLowerCase();

    let func;
    if (type == "postoffice") {
        func = displayPostOfficeModal;
    } else if (type == "storagecompany") {
        func = displayStorageCompanyModal;
    } else if (type == "storage") {
        func = displayStorageModal;
    } else {
        func = displayPostOfficeModal;
    }

    button.addEventListener("click", func);
}

// post office 

async function registerPostOfficeModal() {
    let button = document.querySelector("#addItemButton");
    button.addEventListener("click",
        async function () {
            await displayPostOfficeModal();
        });
}

async function displayPostOfficeModal() {
    let modalWindow = await sendRequest(`/postOffices/modal`);
    document.querySelector("#modalWindowBlock").innerHTML += modalWindow;

    onModalDisplay();
    await registerPostOfficeAdd(onPostOfficeAddModalSubmit);
}

async function registerAddButtonClick(callback) {
    let button = document.querySelector("#create");
    button.addEventListener("click", callback);
}

async function onPostOfficeAddModalSubmit() {
    let form = document.querySelector("#addForm");
    let formData = new FormData(form);
    let parameters = new URLSearchParams(formData);

    let response = await sendRequest("/postOffices", "post", parameters);
    let id = JSON.parse(response);

    if (id) {
        window.location.assign(`/postOffices/${id}/template`);
    }
}


// storage company

async function displayStorageCompanyModal() {
    let modalWindow = await sendRequest(`/storageCompanys/modal`);
    document.querySelector("#modalWindowBlock").innerHTML += modalWindow;

    onModalDisplay();
    await registerStorageCompanyAdd();
}

async function registerStorageCompanyAdd() {
    let button = document.querySelector("#create");
    button.addEventListener("click", onStorageCompanyAddModalSubmit);

    registerAddStorage();
}

async function onStorageCompanyAddModalSubmit() {
    let form = document.querySelector("#addForm");
    let formData = new FormData(form);
    let parameters = new URLSearchParams(formData);
    getStoragesToAdd(parameters);

    let response = await sendRequest("/storageCompanys", "post", parameters);
    let id = JSON.parse(response);

    if (id) {
        window.location.assign(`/storageCompanys/${id}/template`);
    }
}

function registerAddStorage() {
    let button = document.querySelector("#addStorage");
    button.addEventListener("click", onAddStorageClick);

    let storagesSection = document.querySelector("#storages");
    storagesSection.addEventListener("click", onRemoveStorageClick);
}

function onRemoveStorageClick(event) {
    if (event.target.className.includes("removeStorageButton")) {
        let section = event.target.closest(".storage-section");
        document.querySelector("#storages").removeChild(section);
    }
}

async function onAddStorageClick() {
    let storageSection = await sendRequest("/storages/section");
    let storageRow = document.querySelector("#storages");
    let container = document.createElement("div");
    container.setAttribute("class", "row storage-section");
    container.innerHTML = storageSection;
    storageRow.appendChild(container);
}

async function getStoragesToAdd(result) {
    if (!result) {
        result = new URLSearchParams();
    }

    let sections = document.querySelectorAll("#storages .storage-section");

    for (let i = 0; i < sections.length; i++) {
        let form = sections[i].querySelector("form.storage-section-form");
        let formData = new FormData(form);
        let params = new URLSearchParams(formData);
        convertToCollectionElement(params, result, i);
    }
}

function convertUrlParamsToObject(urlParams) {
    let result = {};
    for (let entry of urlParams.entries()) {
        var key = entry[0].replace(/_/g, ".");
        result[key] = entry[1];
    }

    return result;
}

function convertToCollectionElement(urlParams, resultParams, index) {
    for (let entry of urlParams.entries()) {
        let key = `storages[${index}].${entry[0]}`;

        resultParams.append(key, entry[1]);
    }
}

// storage

async function getStorages(type, id) {
    let tableBody = document.querySelector(`#itemsList`);
    let items = await sendRequest(`/${type}/${id}/storages`);
    tableBody.innerHTML = items;
}

///////////////// edit post office section ///////////////

//async function addEditButton(element) {
//    element.addEventListener("click",
//        async function (event) {
//            if (event.target.closest("div").className.includes("edit-button")) {
//                let id = event.target.closest("div.edit-button").id;
//                await onPostOfficeEditButtonClick(id);
//            }

//            if (event.target.closest("div").className.includes("delete-button")) {
//                let id = event.target.closest("div.delete-button").id;
//                await onPostOfficeDeleteButtonClick(id);
//            }
//        });
//}

//async function onPostOfficeEditButtonClick(id) {
//    let modal = await sendRequest(`/postOffices/${id}/modal`);
//    document.querySelector("#modalWindowBlock").innerHTML += modal;

//    onModalDisplay();
//    await registerPostOfficeAdd(onPostOfficeEditModalSubmit);
//}

async function addEditDeleteButton(element, type) {
    let editFunction;
    if (type.toLowerCase() == "postoffices") {
        editFunction = onPostOfficeEditModalSubmit;
    } else if (type.toLowerCase() == "storagecompanys") {
        editFunction = onStorageCompanyEditModalSubmit;
    }

    element.addEventListener("click",
        async function (event) {
            if (event.target.closest("div").className.includes("edit-button")) {
                let id = event.target.closest("div.edit-button").id;
                await onEditButtonClick(id, type, editFunction);
            }

            if (event.target.closest("div").className.includes("delete-button")) {
                let id = event.target.closest("div.delete-button").id;
                await onDeleteButtonClick(id, type);
            }
        });
}

async function onEditButtonClick(id, type, callback) {
    let modal = await sendRequest(`/${type}/${id}/modal`);
    document.querySelector("#modalWindowBlock").innerHTML += modal;

    onModalDisplay();
    await registerAddButtonClick(callback);
}

async function onPostOfficeEditModalSubmit() {
    let form = document.querySelector("#addForm");
    let formData = new FormData(form);
    let parameters = new URLSearchParams(formData);

    await sendRequest("/postOffices", "put", parameters);

    onModalClose();
    getItemsList("postOffices");
}

async function onDeleteButtonClick(id, type) {
    await sendRequest(`/${type}/${id}`, "delete");

    getItemsList(`${type}`);
}

///////////////// edit storage company section ///////////////

async function onStorageCompanyEditModalSubmit() {
    let form = document.querySelector("#addForm");
    let formData = new FormData(form);
    let parameters = new URLSearchParams(formData);

    await sendRequest("/storageCompanys", "put", parameters);

    onModalClose();
    getItemsList("storageCompanys");
}

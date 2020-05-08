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
    let items = await sendRequest(`/${itemType}s/list`);
    tableBody.innerHTML = items;
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
    } else {
        func = displayPostOfficeModal;
    }

    button.addEventListener("click", func);
}

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
    await registerPostOfficeAdd();
}

async function registerPostOfficeAdd() {
    let button = document.querySelector("#create");
    button.addEventListener("click", onPostOfficeAddModalSubmit);
}

async function onPostOfficeAddModalSubmit() {
    var form = document.querySelector("#addForm");
    let formData = new FormData(form);
    let parameters = new URLSearchParams(formData);

    let id = await sendRequest("/postOffices", "post", parameters);
    if (id) {
        window.location.assign(`/Admin/PostOfficeTemplate/${id}`);
    }
}

async function displayStorageCompanyModal() {
    let modalWindow = await sendRequest(`/storageCompany/modal`);
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

    //let id = await sendRequest("/storageCompany", "post", parameters.toString());
    await getStoragesToAdd("7085a8aa-4566-46f3-8971-8dcecf0c741e");
    //if (id) {
    //    window.location.assign(`/Admin/StorageCompany/${id}`);
    //}
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
    let storageSection = await sendRequest("/storage/section");
    //let element = new DOMParser().parseFromString(storageSection, "text/html");
    let storageRow = document.querySelector("#storages");
    let container = document.createElement("div");
    container.setAttribute("class", "row storage-section");
    container.innerHTML = storageSection;
    storageRow.appendChild(container);
}

async function getStoragesToAdd(id) {
    let sections = document.querySelectorAll("#storages .storage-section");
    var result = new URLSearchParams();
    for (let i = 0; i < sections.length; i++) {
        let form = sections[i].querySelector("form.storage-section-form");
        let formData = new FormData(form);
        let params = new URLSearchParams(formData);
        convertToCollectionElement(params, result, i);
        //let object = convertUrlParamsToObject(params);
        //result.push(object);
    }

    //let json = JSON.stringify(result);
    //let headers = {"Content-Type" : "application/json"}
    let response = await sendRequest(`/storageCompanies/${id}/storages`, "post", result);
}

function convertUrlParamsToObject(urlParams) {
    let result = {};
    for (let entry of urlParams.entries()) {
        var key = entry[0].replace(/_/g, ".");
        result[key] = entry[1];
    }

    return result;
}

function convertToCollectionElement(urlParams, resultParams,  index) {
    for (let entry of urlParams.entries()) {
        let key = `storages[${index}].${entry[0]}`;
        //if (key.includes(".")) {
        //    let dotIndex = key.indexOf(".");
        //    let leftPart = `${key.substr(0, dotIndex)}[${index}]`;
        //    let rightPart = key.substr(dotIndex);
        //    key = leftPart + rightPart;
        //} else {
        //    key = `${key}[${index}]`;
        //}

        resultParams.append(key, entry[1]);
    }
}

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

function logout() {
    document.querySelector("#logout").addEventListener("click",
        function (event) {
            event.preventDefault();
            let form = event.target.closest("form");
            form.submit();
        });
}

async function getItemsList(itemType) {
    let tableBody = document.querySelector(`#itemsList`);
    let items = await sendRequest(`/${itemType}`);
    tableBody.innerHTML = items;
}

async function getLatestOrders(id, type) {
    let tableBody = document.querySelector("#latestOrders");
    let orders = await sendRequest(`/${type}/${id}/orders/latest`);
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
            if (event.target.closest(".statusChangeComponent") != null) {
                return;
            }

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
    if (type == "postoffices") {
        func = displayPostOfficeModal;
    } else if (type == "storagecompanys") {
        func = displayStorageCompanyModal;
    } else if (type == "storages") {
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

    let form = document.querySelector("#modalWindowBlock .modal #addForm");
    $.validator.unobtrusive.parse(form);

    onModalDisplay();
    await registerAddButtonClick(onPostOfficeAdd);
}

async function registerAddButtonClick(callback) {
    let form = document.querySelector("#modalWindowBlock .modal #addForm");
    form.addEventListener("submit", callback);
}

async function onPostOfficeAdd(event) {
    event.preventDefault();
    if (!$("#modalWindowBlock .modal #addForm").valid()) {
        return;
    }

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

    let form = document.querySelector("#modalWindowBlock .modal #addForm");
    $.validator.unobtrusive.parse(form);

    onModalDisplay();
    await registerAddButtonClick(onStorageCompanyAdd);
    registerAddStorage();
}

async function onStorageCompanyAdd() {
    event.preventDefault();
    if (!$("#modalWindowBlock .modal #addForm").valid()) {
        return;
    }

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

const collectionItemsElementModule = (function () {
    let _requestUrl = "";
    let _sectionSelector = "";
    let _elementSelector = "";

    async function onItemAddClick() {
        let itemSection = await sendRequest(_requestUrl);

        let container = document.createElement("div");
        container.setAttribute("class", `row ${_elementSelector}`);
        container.innerHTML = itemSection;

        let parentElement = document.querySelector(_sectionSelector);
        parentElement.appendChild(container);
    }

    function registerCollectionItemsAddButton(addButtonSelector, requestUrl, sectionSelector, elementSelector) {
        _requestUrl = requestUrl;
        _sectionSelector = sectionSelector;
        _elementSelector = elementSelector;

        let button = document.querySelector(addButtonSelector);
        button.addEventListener("click", onItemAddClick);
    }

    return {
        registerAddButton: registerCollectionItemsAddButton
    };
})();

const collectionItemsModule = (function () {
    const formSelector = "#addForm";
    let _sectionSelector = "";
    let _elementSelector = "";
    let _elementName = "";
    let _submitEventCallback;

    async function onFormSubmit(event) {
        event.preventDefault();

        let form = document.querySelector(formSelector);
        let formData = new FormData(form);
        getSections(form, formData);
        let parameters = new URLSearchParams(formData);

        _submitEventCallback(parameters);
    }

    function getSections(formElement, formData) {
        let sections = formElement.querySelectorAll(`${_sectionSelector} .${_elementSelector}`);

        for (let i = 0; i < sections.length; i++) {
            let elements = sections[i].querySelectorAll(".form-group input");
            for (let j = 0; j < elements.length; j++) {
                let name = elements[j].name;
                let value = elements[j].value;
                let key = `${_elementName}[${i}].${name}`;
                formData.append(key, value);
            }
        }
    }

    async function registerFormSubmitEvent(sectionSelector, elementSelector, elementName, submitEventCallback) {
        _sectionSelector = sectionSelector;
        _elementSelector = elementSelector;
        _elementName = elementName;
        _submitEventCallback = submitEventCallback;

        let form = document.querySelector(formSelector);
        form.addEventListener("submit", onFormSubmit);
    }

    return {
        registerOnSubmitEvent: registerFormSubmitEvent
    }
})();

const submitEventHandlers = (function () {
    async function onPriceCalculateSubmit(parameters) {
        let response = await sendRequest("/price", "post", parameters);

        let resultElement = document.querySelector("#priceResult");
        resultElement.innerHTML = response;

        document.querySelector(".main-content").scrollIntoView({behavior: "smooth"});
    }

    return {
        onPriceCalculate: onPriceCalculateSubmit
    };
})();

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

async function displayStorageModal() {
    var id = document.querySelector("#Id").value;
    var type = document.querySelector("#Type").value;
    let modalWindow = await sendRequest(`/storages/${type}/${id}/modal`);
    document.querySelector("#modalWindowBlock").innerHTML += modalWindow;

    //get storageCompanies for select list
    let response = await sendRequest("/storageCompanys/json");
    let storageCompanys = JSON.parse(response);

    let selectElement = document.querySelector("#modalWindowBlock .modal #storage-company");
    for (let storageCompany of storageCompanys) {
        let option = `<option value="${storageCompany.id}">${storageCompany.name}</option>`;
        selectElement.innerHTML += option;
    }

    selectElement.addEventListener("change", onStorageCompanySelected);
    // add event listener to select and load storages on changes

    onModalDisplay();
    if (type == "postOffices") {
        await registerAddButtonClick(onStorageToPostOfficeAdd);
    } else {
        await registerAddButtonClick(onStorageToStorageCompanyAdd);
    }
}

async function onStorageToPostOfficeAdd() {
    let parameters = {};
    let selectStorageElement = document.querySelector("#modalWindowBlock .modal #storage");
    let storageId = selectStorageElement.options[selectStorageElement.selectedIndex].value;
    if (!storageId) {
        let selectCompanyElement = document.querySelector("#modalWindowBlock .modal #storage-company");
        let companyId = selectCompanyElement.options[selectCompanyElement.selectedIndex].value;
        if (!companyId) {
            return;
        }

        parameters["StorageCompanyId"] = companyId;
    } else {
        parameters["Id"] = storageId;
    }

    let body = JSON.stringify(parameters);
    let headers = {'Content-Type': 'application/json'};
    var postOfficeId = document.querySelector("#Id").value;

    await sendRequest(`/postOffices/${postOfficeId}/storages`, "post", body, headers);
}

function onStorageToStorageCompanyAdd() {

}

async function onStorageCompanySelected(event) {
    let selectElement = document.querySelector("#modalWindowBlock .modal #storage");

    cleanSelectList(selectElement);
    selectElement.closest("div").classList.add("disabled");
    let companyId = event.target.options[event.target.selectedIndex].value;
    if (!companyId) {
        return;
    }

    let response = await sendRequest(`/storageCompanys/${companyId}/storages/json`);
    let storages = JSON.parse(response);

    for (let storageCompany of storages) {
        let option = `<option value="${storageCompany.id}">${storageCompany.name}</option>`;
        selectElement.innerHTML += option;
    }

    selectElement.closest("div.disabled").classList.remove("disabled");
}

function cleanSelectList(selectElement) {
    for (let i = selectElement.options.length - 1; i > 0; i--) {
        selectElement.remove(i);
    }
}

///////////////// edit post office section ///////////////

async function addEditDeleteButton(element, type) {
    let editFunction;
    if (type.toLowerCase() == "postoffices") {
        editFunction = onPostOfficeEdit;
    } else if (type.toLowerCase() == "storagecompanys") {
        editFunction = onStorageCompanyEdit;
    }

    element.addEventListener("click",
        async function (event) {
            if (event.target.closest("div").className.includes("edit-button")) {
                let id = event.target.closest("div.edit-button").id;
                await getEditModal(id, type, editFunction);
            }

            if (event.target.closest("div").className.includes("delete-button")) {
                let id = event.target.closest("div.delete-button").id;
                await onDelete(id, type);
            }
        });
}

async function getEditModal(id, type, callback) {
    let modal = await sendRequest(`/${type}/${id}/modal`);
    document.querySelector("#modalWindowBlock").innerHTML += modal;

    let form = document.querySelector("#modalWindowBlock .modal #addForm");
    $.validator.unobtrusive.parse(form);

    onModalDisplay();
    await registerAddButtonClick(callback);
}

async function onPostOfficeEdit() {
    event.preventDefault();
    if (!$("#modalWindowBlock .modal #addForm").valid()) {
        return;
    }

    let form = document.querySelector("#addForm");
    let formData = new FormData(form);
    let parameters = new URLSearchParams(formData);

    await sendRequest("/postOffices", "put", parameters);

    onModalClose();
    getItemsList("postOffices");
}

async function onDelete(id, type) {
    await sendRequest(`/${type}/${id}`, "delete");

    getItemsList(`${type}`);
}

///////////////// edit storage company section ///////////////

async function onStorageCompanyEdit() {
    event.preventDefault();
    if (!$("#modalWindowBlock .modal #addForm").valid()) {
        return;
    }

    let form = document.querySelector("#addForm");
    let formData = new FormData(form);
    let parameters = new URLSearchParams(formData);

    await sendRequest("/storageCompanys", "put", parameters);

    onModalClose();
    getItemsList("storageCompanys");
}

////////////// statistics ////////////

async function getActiveStorages(type, id) {
    let response = await sendRequest(`/${type}/${id}/storages/json`);
    let storages = JSON.parse(response);

    let element = document.querySelector("h2#storagesCount");
    let textElement = document.createTextNode(storages.length);
    element.appendChild(textElement);
}

async function getOrdersStatistics(type, id) {
    let response = await sendRequest(`/${type}/${id}/orders/json`);
    let orders = JSON.parse(response);

    let countElement = document.querySelector("h2#ordersCount");
    let textCountElement = document.createTextNode(orders.length);
    countElement.appendChild(textCountElement);

    let sum = 0;
    for (let order of orders) {
        sum += parseFloat(order.price);
    }

    let sumElement = document.querySelector("h2#ordersSum");
    let textSumElement = document.createTextNode(sum);
    sumElement.appendChild(textSumElement);
}

//////////////// account page //////////////

async function getUserOrders() {
    let orders = await sendRequest(`/users/orders`);

    let tableBody = document.querySelector("#orders");
    tableBody.innerHTML = orders;
}

async function getPostOffices() {
    let postOfficeSelect = document.querySelector("#PostOfficeId");

    await getPostOfficesSelectList(postOfficeSelect);

    postOfficeSelect.addEventListener("change", onPostOfficeSelected);
}

async function getPostOfficesSelectList(selectElement) {
    let response = await sendRequest("/postOffices/json");
    let postOffices = JSON.parse(response);
    for (let postOffice of postOffices) {
        let option = `<option value="${postOffice.id}">${postOffice.name}</option>`;
        selectElement.innerHTML += option;
    }
}

async function onPostOfficeSelected(event) {
    let sourceStorages = document.querySelector("#SourceStorageId");
    let destinationStorages = document.querySelector("#DestinationStorageId");
    let sourceCities = document.querySelector("#sourceCity");
    let destinationCities = document.querySelector("#destinationCity");

    cleanSelectList(sourceStorages);
    cleanSelectList(destinationStorages);
    cleanSelectList(sourceCities);
    cleanSelectList(destinationCities);

    let postOfficeId = event.target.options[event.target.selectedIndex].value;
    if (!postOfficeId) {
        return;
    }

    let response = await sendRequest(`/postOffices/${postOfficeId}/storages/json`);
    let storages = JSON.parse(response);

    let sourceCitySelect = document.querySelector("#sourceCity");
    let destinationCitySelect = document.querySelector("#destinationCity");
    let cities = storages.map(x => x.address.city).filter((value, index, self) => self.indexOf(value) === index);
    for (let city of cities) {
        let option = `<option value="${city}">${city}</option>`;
        sourceCitySelect.innerHTML += option;
        destinationCitySelect.innerHTML += option;
    }

    sourceCitySelect.addEventListener("change",
        async function (event) {
            onCitySelected(event, storages, sourceStorages);
        });

    destinationCitySelect.addEventListener("change",
        async function (event) {
            onCitySelected(event, storages, destinationStorages);
        });
}

async function onCitySelected(event, storages, addressSelect) {
    let selectedCity = event.target.options[event.target.selectedIndex].value;
    let storageListByCity = [];
    for (let storage of storages) {
        if (storage.address.city == selectedCity) {
            storageListByCity.push(storage);
        }
    }

    cleanSelectList(addressSelect);
    for (let storage of storageListByCity) {
        let option = `<option value="${storage.id}">${storage.address.street}, ${storage.address.house}</option>`;
        addressSelect.innerHTML += option;
    }

}

function checkDestinationUser() {
    let input = document.querySelector("#DestinationClientEmail");
    input.addEventListener("change", onUserDestinationSelected);
}

async function onUserDestinationSelected(event) {
    let email = event.target.value;
    let response = await sendRequest(`/users/${email}`);

    document.querySelector("#userSection").classList.add("disabled");
    if (!response) {
        let span = document.querySelector("span[data-valmsg-for='DestinationClientEmail']");
        span.classList.remove("field-validation-valid");
        span.classList.add("field-validation-no-valid");

        return;
    }

    let user = JSON.parse(response);
    document.querySelector("#userFirstName").value = user.firstName;
    document.querySelector("#userLastName").value = user.lastName;

    document.querySelector("#userSection").classList.remove("disabled");
}

async function addUserOrderDeleteButton(element) {
    element.addEventListener("click",
        async function (event) {
            if (event.target.closest("div").className.includes("delete-button")) {
                let id = event.target.closest("div.delete-button").id;
                await onUserOrderDelete(id);
            }
        });
}

async function onUserOrderDelete(id) {
    await sendRequest(`/orders/${id}`, "delete");

    await getUserOrders();
}

function registerOrderSearch() {
    let button = document.querySelector("#searchButton");
    button.addEventListener("click", onSearchOrders);

    let cleanButton = document.querySelector("#cleanButton");
    cleanButton.addEventListener("click", onSearchClean);
}

async function onSearchOrders() {
    let searchValue = document.querySelector("#searchText").value;
    let orders = await sendRequest(`/orders/search/${searchValue}`);

    let tableBody = document.querySelector("#orders");
    tableBody.innerHTML = orders;
}

async function onSearchClean() {
    let id = document.querySelector("#Id").value;
    let type = document.querySelector("#Type").value;
    getOrders(id, type);
}

const priceModule = (function () {
    async function registerPricePageEvents() {
        let postOfficeSelect = document.querySelector("#PostOfficeId");

        await getPostOfficesSelectList(postOfficeSelect);

        postOfficeSelect.addEventListener("change", onPostOfficeSelected);
    }

    async function onPostOfficeSelected(event) {
        let postOfficeId = event.target.options[event.target.selectedIndex].value;
        if (!postOfficeId) {
            return;
        }

        let response = await sendRequest(`/postOffices/${postOfficeId}/storages/json`);
        let storages = JSON.parse(response);

        let cities = storages.map(x => x.address.city).filter((value, index, self) => self.indexOf(value) === index);

        let sourceCity = document.querySelector("#SourceAddress_City");
        setupCitySelectList(sourceCity, cities);

        let destinationCity = document.querySelector("#DestinationAddress_City");
        setupCitySelectList(destinationCity, cities);
    }

    function setupCitySelectList(selectElement, cities) {
        cleanSelectList(selectElement);

        for (let city of cities) {
            let option = `<option value="${city}">${city}</option>`;
            selectElement.innerHTML += option;
        }
    }

    return {
        registerEvents: registerPricePageEvents
    };
})();

const userSearchModule = (function () {
    function initialize(elementSelector) {
        $(elementSelector).select2();
        $(elementSelector).on("select2:open", async function () {
            let element = document.querySelector(".select2-search__field");
            element.addEventListener("input", async function (event) {
                let value = event.target.value;
                await getUsers(value, elementSelector);
            });
        });
    }

    async function getUsers(value, elementSelector) {
        // let data = await sendRequest(`${value}`);
        let data = [
            {
                id: 1,
                text: "sdsdsd"
            },
            {
                id: 2,
                text: "asasas"
            }
        ];

        $(elementSelector).val(null).trigger('change');

        for (const element of data) {
            let newOption = new Option(element.text, element.id, false, false);
            $(elementSelector).append(newOption).trigger('change');
        }
    }

    return {
        init: initialize
    };
})();

const searchableDropdownModule = (function () {
    let _input = document.querySelector("#search-field");
    let _container = document.querySelector("#search-options-container");

    function initialize() {
        _input.addEventListener("input", onInput);
        _container.addEventListener("click", onItemClick);
        displayListElement("", "Enter user name");
    }

    async function onInput(event) {
        let value = event.target.value;
        removeChildNodes();

        if (!value) {
            displayListElement("", "Enter user name");
            return;
        }

        let users = await getUsers(value);
        populateDropdown(users);
    }

    function populateDropdown(users) {
        for (const user of users) {
            displayListElement(user.id, `${user.firstName} ${user.lastName}`);
        }
    }

    function onItemClick(event) {
        if (event.target.classList.contains("dropdown-item")) {
            _input.value = event.target.dataset.text;
            _input.dataset.id = event.target.dataset.id;
        }
    }

    function displayListElement(id, text) {
        let element = document.createElement("p");
        element.setAttribute("class", "dropdown-item");
        element.dataset.id = id;
        element.dataset.text = text;

        let textNode = document.createTextNode(text);
        element.appendChild(textNode);

        _container.appendChild(element);
    }

    function removeChildNodes() {
        while (_container.firstChild) {
            _container.removeChild(_container.lastChild);
        }
    }

    async function getUsers(searchParameter) {
        let response = await sendRequest(`/users?name=${searchParameter}`);
        let users = JSON.parse(response)

        return users;
    }

    return {
        init: initialize
    }
})();

const loadModule = (function () {
    function onDocumentLoaded(action) {
        document.addEventListener('DOMContentLoaded', function () {
            action();
        });
    }

    return {
        onDocumentLoaded: onDocumentLoaded
    };
})();
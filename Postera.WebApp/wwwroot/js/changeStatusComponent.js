function registerChangeStatusComponent(table) {
    table.addEventListener("change",
        function (event) {
            let target = event.target;
            if (target.tagName == "SELECT") {
                let row = target.closest("tr");
                displayControls(row);
            }
        });
}

function displayControls(element) {
    let statusComponent = element.querySelector(".statusChangeComponent");
    let controlsBlock = createControls(statusComponent);
    statusComponent.appendChild(controlsBlock);
}

function createControls(statusComponent) {
    let selectList = statusComponent.querySelector("select");
    let statusInitialValue = selectList.options[selectList.selectedIndex].value;

    let selectControls = document.createElement("div");
    selectControls.classList.add("selectControls");

    let applyButton = document.createElement("button");
    applyButton.id = "applyStatus";
    applyButton.classList.add("form-control", "btn-primary");
    applyButton.value = "Apply";

    let cancelButton = document.createElement("button");
    cancelButton.id = "cancelStatus";
    cancelButton.classList.add("form-control", "btn-group");
    cancelButton.value = "Cancel";
    cancelButton.dataset.initialState = statusInitialValue;
    cancelButton.addEventListener("click", onCancel);

    selectControls.appendChild(applyButton);
    selectControls.appendChild(cancelButton);
}

function onApply(event) {
    let orderId = event.target.closest("tr").id;
    let statusComponent = event.target.closest(".statusChangeComponent");
    let selectList = statusComponent.querySelector("select");
    let statusValue = selectList.options[selectList.selectedIndex].value;

    await sendRequest(`orders/${orderId}/${statusValue}`);

    removeControls(statusComponent);
}

function onCancel(event) {
    let statusComponent = event.target.closest(".statusChangeComponent");
    removeControls(statusComponent);
}

function removeControls(statusComponent) {
    let controlsBlock = statusComponent.querySelector(".selectControls");
    let cancelButton = statusComponent.querySelector("#cancelStatus");
    let statusInitialValue = cancelButton.dataset.initialState;

    let selectList = statusComponent.querySelector("select");
    selectList.querySelector(`option[value=${statusInitialValue}]`).selected = "selected";

    statusComponent.removeChild(controlsBlock);
}
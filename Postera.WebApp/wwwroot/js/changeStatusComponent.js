function registerChangeStatusComponent(table) {
    table.addEventListener("change",
        function (event) {
            let target = event.target;
            if (target.tagName == "SELECT") {
                event.stopPropagation();
                let row = target.closest("tr");

                displayControls(row);
            }
        });
}

function displayControls(element) {
    let statusComponent = element.querySelector(".statusChangeComponent");
    removeControls(statusComponent);

    let controlsBlock = createControls();
    statusComponent.appendChild(controlsBlock);
}

function createControls() {
    let selectControls = document.createElement("div");
    selectControls.classList.add("selectControls", "form-group");

    let applyButton = document.createElement("button");
    applyButton.id = "applyStatus";
    applyButton.classList.add("form-control", "btn-primary");
    applyButton.addEventListener("click", onApply);

    let applyText = document.createTextNode("Apply");
    applyButton.appendChild(applyText);

    let cancelButton = document.createElement("button");
    cancelButton.id = "cancelStatus";
    cancelButton.classList.add("form-control", "btn-light");
    cancelButton.addEventListener("click", onCancel);

    let cancelText = document.createTextNode("Cancel");
    cancelButton.appendChild(cancelText);

    selectControls.appendChild(applyButton);
    selectControls.appendChild(cancelButton);

    return selectControls;
}

async function onApply(event) {
    let orderId = event.target.closest("tr").id;
    let statusComponent = event.target.closest(".statusChangeComponent");
    let selectList = statusComponent.querySelector("select");
    let statusValue = selectList.options[selectList.selectedIndex].value;

    await sendRequest(`/orders/${orderId}/${statusValue}`, "post");

    removeControls(statusComponent);
    event.stopPropagation();
}

function onCancel(event) {
    let statusComponent = event.target.closest(".statusChangeComponent");
    removeControls(statusComponent);
    event.stopPropagation();
}

function removeControls(statusComponent) {
    let controlsBlock = statusComponent.querySelector(".selectControls");
    if (controlsBlock == null) {
        return;
    }

    let selectList = statusComponent.querySelector("select");
    let statusInitialValue = selectList.dataset.initial_state;
    selectList.querySelector(`option[value=${statusInitialValue}]`).selected = "selected";

    statusComponent.removeChild(controlsBlock);
}
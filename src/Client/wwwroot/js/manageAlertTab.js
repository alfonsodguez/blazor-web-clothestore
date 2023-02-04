window.alert = (message, type, isvisible) => {
    var isExistAlertTab = document.getElementById("alertId")

    if (isvisible) {
        if (isExistAlertTab === null || isExistAlertTab === undefined) {
            var wrapper          = document.createElement('div')
            wrapper.innerHTML    = '<div id="alertId" class="alert alert-' + type + ' alert-dismissible" role="alert">' + message + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>'
            var alertPlaceholder = document.getElementById('liveAlertPlaceholder')
            alertPlaceholder.append(wrapper)
        }
    }
    else if (isExistAlertTab != null || isExistAlertTab != undefined) {
        isExistAlertTab.remove()
    }
}

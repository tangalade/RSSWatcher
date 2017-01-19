/* Define in including file */
var filterControlPath = "";
var sortControlPath = "";
var pageControlPath = "";

var sortableCols = [];
var filterableCols = [];

var filterControls = {};
var sortControls = {};
var pageControls = {};

var controlSubmitURL = "";

String.prototype.inArray = function (array) {
    for (var i = 0; i < array.length; i++) {
        if (array[i] == this)
            return true;
    }
    return false;
}

function gotoPage(page) {
    pageControls['PageNum'] = page;

    genAndSubmitControlForm();
}

function setPageSize(pageSize) {
    pageControls['PageSize'] = pageSize;

    genAndSubmitControlForm();
}

function removeFilter(col) {
    if (filterControls[col]) {
        delete filterControls[col];

        genAndSubmitControlForm();
    }
}

function sortBy(col) {
    if (col.inArray(sortableCols)) {
        if (sortControls['Name'] == col) {
            if (sortControls['Ascending'] == "True")
                sortControls['Ascending'] = "False";
            else
                sortControls['Ascending'] = "True";
        } else {
            sortControls['Name'] = col;
            sortControls['Ascending'] = "True";
        }

        genAndSubmitControlForm();
    }
}

function filterBy(col, value) {
    if (col.inArray(filterableCols)) {
        filterControls[col] = value;

        genAndSubmitControlForm();
    }
}

function genAndSubmitControlForm() {
    var form = genControlForm();
    document.body.appendChild(form);
    form.submit();
}

function genControlForm() {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", controlSubmitURL);
    addHiddenControlFields(form);
    return form;
}

function addHiddenControlFields(form) {
    var idx = 0;
    for (var key in filterControls) {
        if (filterControls.hasOwnProperty(key)) {
            var hiddenNameField = genHiddenField(filterControlPath + "[" + idx + "].Name", key);
            form.appendChild(hiddenNameField);
            console.log(filterControlPath + "[" + idx + "].Name" + ": " + key)
            var hiddenValueField = genHiddenField(filterControlPath + "[" + idx + "].Value", filterControls[key]);
            form.appendChild(hiddenValueField);
            console.log(filterControlPath + "[" + idx + "].Value" + ": " + filterControls[key])
            idx++;
        }
    }
    for (var key in sortControls) {
        if (sortControls.hasOwnProperty(key)) {
            var hiddenField = genHiddenField(sortControlPath + "." + key, sortControls[key]);
            form.appendChild(hiddenField);
            console.log(sortControlPath + "." + key + ": " + sortControls[key])
        }
    }
    for (var key in pageControls) {
        if (pageControls.hasOwnProperty(key)) {
            var hiddenField = genHiddenField(pageControlPath + "." + key, pageControls[key]);
            form.appendChild(hiddenField);
            console.log(pageControlPath + "." + key + ": " + pageControls[key])
        }
    }
}

function genHiddenField(name, value) {
    var hiddenField = document.createElement("input");
    hiddenField.setAttribute("type", "hidden");
    hiddenField.setAttribute("name", name);
    hiddenField.setAttribute("value", value);
    return hiddenField;
}
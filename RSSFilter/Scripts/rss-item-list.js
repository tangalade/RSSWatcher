var editURL = "";
var markNewURL = "";
var markReadURL = "";
var archiveURL = "";
var viewItemURL = "";
var rateArtistURL = "";
var rateItemTypeURL = "";
var rssItemCheckboxes = [];

function clickPageControl() {
    var pageControl = this.textContent;
    console.log("pageControl." + pageControl + " clicked");

    switch (pageControl) {
        case "Previous":
            gotoPage(+pageControls['PageNum'] - 1);
            break;
        case "Next":
            gotoPage(+pageControls['PageNum'] + 1);
            break;
        default:
            gotoPage(pageControl);
            break;
    }
}

function clickPageSize() {
    var pageSize = this.textContent;
    console.log("pageSize." + pageSize + " clicked");

    setPageSize(pageSize);
}

function clickRemoveFilter() {
    var col = this.dataset.col;
    console.log("filter." + col + " clicked");

    removeFilter(col);
}

function clickSort() {
    var col = this.dataset.col;
    console.log("header." + col + " clicked");

    sortBy(col);
}

function clickFilter() {
    var col = this.closest("td").dataset.col;
    var value = this.textContent;
    console.log("item." + col + " clicked with value: " + value);

    filterBy(col, value);
}

// RSS Item specific stuff
function clickActionButton() {
    var viewStatus = this.dataset.viewstatus;
    console.log("action." + viewStatus + " clicked");

    var selectedIds = [];
    for (var i = 0; i < rssItemCheckboxes.length; i++) {
        if (rssItemCheckboxes[i].checked)
            selectedIds.push(rssItemCheckboxes[i].dataset.id);
    }
    if (selectedIds.length == 0)
        return;

    var form = genControlForm(markViewStatusURL);

    for (var i = 0; i < selectedIds.length; i++) {
        var idField = genHiddenField("ids[" + i + "]", selectedIds[i]);
        form.appendChild(idField);
    }
    var viewStatusField = genHiddenField("viewStatus", viewStatus);
    form.appendChild(viewStatusField);

    document.body.appendChild(form);
    form.submit();
}
//function clickActionButton() {
//    var action = this.dataset.action;
//    console.log("action." + action + " clicked");

//    var selectedIds = [];
//    for (var i = 0; i < rssItemCheckboxes.length; i++) {
//        if (rssItemCheckboxes[i].checked)
//            selectedIds.push(rssItemCheckboxes[i].dataset.id);
//    }
//    if (selectedIds.length == 0)
//        return;

//    var submitURL = (action == "MarkNew") ? markNewURL :
//        (action == "MarkViewed") ? markViewedURL :
//        (action == "Archive") ? archiveURL : null;

//    var form = genControlForm(submitURL);

//    for (var i = 0; i < selectedIds.length; i++) {
//        var idField = genHiddenField("ids[" + i + "]", selectedIds[i]);
//        form.appendChild(idField);
//    }

//    document.body.appendChild(form);
//    form.submit();
//}

function clickThumb() {
    var active = this.dataset.active;
    var type = this.dataset.type;
    var artistId = this.dataset.artistid;
    var itemTypeId = this.dataset.itemtypeid;
    if (itemTypeId != null)
        console.log("thumb." + type + " clicked with value: " + active + " for itemtype: " + itemTypeId);
    if (artistId != null)
        console.log("thumb." + type + " clicked with value: " + active + " for artist: " + artistId);
    var id = (itemTypeId != null) ? itemTypeId : (artistId != null) ? artistId : null;
    var submitURL = (itemTypeId != null) ? rateItemTypeURL : (artistId != null) ? rateArtistURL : null;

    if (id == null || submitURL == null)
        return;

    var form = genControlForm(submitURL);

    var idField = genHiddenField("id", id);
    form.appendChild(idField);

    var rating;
    if (active == "True") {
        rating = 0;
    } else if (type == "up") {
        rating = 1;
    } else if (type == "down") {
        rating = 2;
    }
    var ratingField = genHiddenField("rating", rating);
    form.appendChild(ratingField);

    document.body.appendChild(form);
    form.submit();
}

function clickTitle() {
    var title = this.textContent;
    var url = this.href;
    var id = this.dataset.id;

    console.log("title clicked for item: " + id);

    var form = genControlForm(viewItemURL);
    console.log("submitting form to " + viewItemURL)
    var idField = genHiddenField("id", id);
    form.appendChild(idField);

    document.body.appendChild(form);
    form.submit();
}

function clickEdit() {
    var id = this.dataset.id;
    console.log("edit clicked for item: " + id);

    window.location.href = editURL + "/" + id;
}

function clickArchive() {
    var id = this.dataset.id;
    console.log("archive clicked for item: " + id);

    var form = genControlForm(archiveURL);
    
    var idField = genHiddenField("ids[0]", id);
    form.appendChild(idField);

    document.body.appendChild(form);
    form.submit();
}

function clickHeadSelect() {
    // the event handler happens before the checkbox actually toggles
    var newChecked = this.checked;
    for (var i = 0; i < rssItemCheckboxes.length; i++) {
        rssItemCheckboxes[i].checked = newChecked;
    }
}
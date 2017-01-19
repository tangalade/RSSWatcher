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
    var action = this.dataset.action;
    console.log("action." + action + " clicked");

    switch (action) {
        case "MarkNew":
            break;
        case "MarkRead":
            break;
        case "Archive":
            break;
    }
}

function clickThumb() {
    var active = this.dataset.active;
    var type = this.dataset.type;
    var artistid = this.dataset.artistid;
    var itemtypeid = this.dataset.itemtypeid;
    if (itemtypeid != null)
        console.log("thumb." + type + " clicked with value: " + active + " for itemtype: " + itemtypeid);
    if (artistid != null)
        console.log("thumb." + type + " clicked with value: " + active + " for artist: " + artistid);

    if (type == "up") {
        if (active == "True") {

        } else {

        }
    } else {
        if (active == "True") {

        } else {

        }
    }
}

function clickTitle() {
    var title = this.textContent;
    var url = this.href;
    var id = this.dataset.itemid;

    console.log("title clicked for item: " + id);

    // have the controller update the item as viewed, then go to url
}

function clickEdit() {
    var id = this.dataset.itemid;
    console.log("edit clicked for item: " + id);

}

function clickArchive() {
    var id = this.dataset.itemid;
    console.log("archive clicked for item: " + id);

}

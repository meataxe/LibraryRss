/*global $, kendo, console, LIBRARYRSS*/

LIBRARYRSS.namespace("LIBRARYRSS.pages");

LIBRARYRSS.pages.initTest = function(data) {
  'use strict';

  var initGrid,
      initScoreGrid,
      initScoreWindow,
      changeGrid,
      setGridStateFromCookie,
      setGridStateCookie,
      gridHeight = $(window).height() - 276,
      initialStateSet = false;

  changeGrid = function(e) {
    if (!initialStateSet) {
      return;
    }

    var state = JSON.stringify({
      filter: e.sender._filter,
      sort: e.sender._sort
    });

    console.log(state);
    setGridStateCookie(state);
  };

  setGridStateFromCookie = function () {
    var state = $.cookie("mb-libraryrss-home-test-gridstate")
      || '{"filter":{"filters":[{"field":"IsFiction","operator":"eq","value":"Yes"},{"field":"Score","operator":"gte","value":0}],"logic":"and"},"sort":[{"field":"ShelfLocation","dir":"asc"}]}';

    var data = JSON.parse(state);
    $("#title-table").data("kendoGrid").dataSource.query(data);
    setGridStateCookie(state);
    
    initialStateSet = true;
  };

  setGridStateCookie = function (state) {
    $.cookie("mb-libraryrss-home-test-gridstate", state, { expires: 1000, path: '/' });
  };
  
  initGrid = function (data) {
    $("#title-table").kendoGrid({
      dataSource: {
        data: data,
        change: changeGrid,
        schema: {
          type: "json",
          model: {
            fields: {
              Title: { type: "string" },
              Author: { type: "string" },
              Id: { type: "string" },
              TitleUrl: { type: "string" },
              DatePublished: { type: "date" },
              Content: { type: "string" },
              Isbn: { type: "string" },
              SubjectTermsString: { type: "string" },
              ImageUrl: { type: "string" },
              ShelfLocation: { type: "string" },
              SmallImageUrl: { type: "string" },
              Score: { type: "number" },
              IsFiction: { type: "string" }
            }
          }
        }        
      },
      height: gridHeight,
      scrollable: true,
      sortable: true,
      filterable: { extra: false },                    
      navigatable: true,
      reorderable: false,
      columns: [{
        field: "Title",
        template: "<a target='_' href='#=TitleUrl#'>#=Title#</a>"
      }, {
        field: "Author"
      }, {
        field: "SubjectTermsString",
        title: "Subject"
      }, {
        field: "ShelfLocation",
        title: "Shelf Location"
      }, {
        field: "IsFiction",
        title: "Fiction"
      }, {
        field: "Score",
        title: "Score"            
      }]
    });

    setGridStateFromCookie();
  };
  
  initScoreWindow = function () {
    $("#show-score-table").bind("click", function () {
      initScoreGrid();
      $("#score-window").data("kendoWindow").open();
    });

    $("#score-table-cancel").bind("click", function() {
      $("#score-window").data("kendoWindow").close();
    });
    
    $("#score-table-save").bind("click", function () {
      var scores = { TextScores: $("#score-table").data("kendoGrid").dataSource.data() };
      var data = JSON.stringify(scores);

      // .toJSON()
      $.cookie("mb-libraryrss-home-test-scorelist", data, { expires: 1000, path: '/' });
      
      $("#score-window").data("kendoWindow").close();
      
      location.reload();
    });
    
    $("#score-table-help").bind("click", function () {
      $("#help-window").data("kendoWindow").open();
    });

    $("#score-window").kendoWindow({
      width: "600px",
      title: "Title Score Options",
      resizable: false,
      visible: false,
      position: {
        top: 120,
        left: $(window).width() / 2 - 300
      },
      actions: [
        "Close"
      ]
    });
    
    $("#help-window").kendoWindow({
      width: "600px",
      title: "How to use this list",
      resizable: false,
      visible: false,
      modal: true,
      position: {
        top: 120,
        left: $(window).width() / 2 - 300
      },
      actions: [
        "Close"
      ]
    });         
  };
  
  initScoreGrid = function () {
    var data = JSON.parse($.cookie("mb-libraryrss-home-test-scorelist") || "{ TextScores: [] }").TextScores;

    $("#score-table").kendoGrid({
      dataSource: {
        data: data,
        schema: {
          type: "json",
          model: {
            fields: {
              Text: { type: "string" },
              Score: { type: "number" }
            }
          }
        }
      },
      height: "300px",
      scrollable: true,
      sortable: true,
      filterable: false,
      navigatable: false,
      reorderable: false,
      toolbar: ["create"],
      editable: "inline",
      columns: [
        { field: "Text" },
        { field: "Score" },
        { command: ["edit", "destroy"], title: "&nbsp;", width: "160px" }
      ]
    });
  };

  initGrid(data);

  initScoreGrid();
  initScoreWindow();  
};

// get help window working

// This is the app namespace, so we don't end up just dumping everything in 
// window.* - with this, it's effectively window.LIBRARYRSS.*
var LIBRARYRSS = LIBRARYRSS || {};

LIBRARYRSS.namespace = function (ns_string) {
  'use strict';

  var parts = ns_string.split('.'),
      parent = LIBRARYRSS,
      i;

  // strip redundant leading global
  if (parts[0] === "LIBRARYRSS") {
    parts = parts.slice(1);
  }

  for (i = 0; i < parts.length; i += 1) {
    // create a property if it doesn't exist
    if (typeof parent[parts[i]] === "undefined") { //ignore jslint
      parent[parts[i]] = {};
    }

    parent = parent[parts[i]];
  }

  return parent;
};
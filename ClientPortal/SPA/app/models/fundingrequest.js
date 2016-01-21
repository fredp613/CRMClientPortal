// import DS from 'ember-data';
import Ember from 'ember';

// export default DS.Model.extend({
var fr = Ember.Object.extend({
  createFR: function() {
  	return new Promise(function(resolve, reject){
  		if (1 === 1) {
  			resolve("success");
  		} else {
  			reject("no success")
  		}
  	});

  },
  updateFR: function() {
  	console.log("updating funding request")
  },
  getFR: function() {
  	console.log("getting funding request")
  },
  deleteFR: function() {
  	console.log("deleting funding request")
  },
  getAllFrByUser: function() {
  	console.log("getting all funding request")
  }
});


export default fr;
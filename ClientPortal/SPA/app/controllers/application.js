import Ember from 'ember';
import session from "ember-simple-auth/services/session";

export default Ember.Controller.extend({
  	session: Ember.inject.service('session'),
  	actions: {
	    invalidateSession() {
	      this.get('session').invalidate();
	    },
	    getCurrentUser() {
	      console.log("dsaf")
	    }		
  	}
});
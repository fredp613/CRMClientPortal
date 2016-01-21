import Ember from 'ember';
import ESASession from "ember-simple-auth/services/session";
import User from '../models/user';

export default ESASession.extend({
  store: Ember.inject.service(),
  session: Ember.inject.service('session'),

  setCurrentUser: function() {

    if (this.get('isAuthenticated')) {
    			
      	return new Ember.RSVP.Promise(resolve => {
      		var user = User.create({
		 		session: this.get('session'),
		 		store: this.get('store'),
		 		token: this.get('session').get('content')["authenticated"]["access_token"],
		 		target: this,
		 		email: "hi@gmail.com"
			});
			user.setCurrentUser();
	    });
    }  
  }.observes('isAuthenticated')
});


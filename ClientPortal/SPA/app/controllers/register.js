import Ember from 'ember';
import User from '../models/user';
import session from "ember-simple-auth/services/session";

export default Ember.Controller.extend({
	session: Ember.inject.service('session'),

	actions: {
		register() {
		  let { identification,firstName,lastName,password } = this.getProperties('identification','firstName','lastName', 'password');
		  this.set('errorMessage', firstName + " " + lastName)
		  var newUser = User.create({
		  	email: identification,
		  	firstName: firstName,
		  	lastName: lastName,
		  	password: password,
		  	confirmPassword: password
		  });
		  var $this = this;
		  newUser.register().then(function(value) {
		  	console.log('success')
		  	console.log(value)
		  	 $this.get('session').authenticate('authenticator:oauth2', newUser.email, newUser.password).catch((reason) => {
       				 $this.set('errorMessage', reason.error || reason);
     		 });
		  }, function(reason) {
		  	console.log(reason)
		  });
		  
		}
	}
});

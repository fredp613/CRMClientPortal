// import DS from 'ember-data';
import Ember from 'ember';
import Session from "ember-simple-auth/services/session";

var user = Ember.Object.extend({
// export default Ember.Object.extend({
	register: function() {
		
		var $this = this;		
		return new Promise(function(resolve, reject){

			console.log("registering")
			var newUser = {
				Email: $this.get("email"),
				Password: $this.get("password"),
				ConfirmPassword: $this.get("confirmPassword"),
				FirstName: $this.get("firstName"),
				LastName: $this.get("lastName")
			}
			
			////////////////ajax request
			Ember.$.ajax({
			      type: 'POST',
			      url: 'http://localhost:60752/api/Account/register',
			      contentType:'application/x-www-form-urlencoded',
			      data: newUser,
			      success: function (data) {
			      	console.log("no error")
			        resolve(data);
			      },
			      error: function (error) {
			      	console.log("error")
			      	var serverMessage = jQuery.parseJSON(error.responseText);
			        reject(serverMessage);	
			      }		      
			 });
		});
	},
	login: function() {
		console.log("logging in");
		 this.get("session").authenticate('authenticator:oauth2', this.get("email"), this.get("password")).catch((reason) => {
        	this.set('errorMessage', reason.error || reason);
      	 });
	},
	setCurrentUser: function() {
		 
		console.log("hi");
			var session = this.get('session');
			var store = this.get('store');
			var token = this.get('token');
			//get the context of the session class - so grab the target passed to this object to set the current user in the session object
			var target = this.get('target');
			Ember.$.ajax({
			      type: 'GET',
			      url: 'http://localhost:60752/api/Account/UserInfo',
			      beforeSend: function(request) {
			         request.setRequestHeader("Authorization", "bearer " + token);
			      },
			      dataType:"json",
			      // headers: {'authorization', 'bearer A5lqMOsu378jnw0HOrh49Hpya1_FPNTzFpZqZwDVLwoAk0dyFYnznmu0zYTaeqx8jmG2ffGPusAQYWEBxlSwPWrKdicrLiuuAytSwjppWuEHX1oqo2N1iAsaoY9hYyX31tf6zCYAu1Yb'},
			      success: function (data) {
			        console.log(data);
			        target.set('currentUser', data);
			      },
			      error: function (request, textStatus, error) {
			        console.log(error);
			      }
			 });
	}
	
});

export default user;


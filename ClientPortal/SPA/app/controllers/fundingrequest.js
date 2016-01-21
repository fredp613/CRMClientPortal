import Ember from 'ember';
import FundingRequest from "../models/fundingrequest"

export default Ember.Controller.extend({
	session: Ember.inject.service('session'),

	programs: [
	    {programName: "Victims Fund", id: "123213-123213123-sdfaf-123213" },
	    {programName: "Victims Fund", id: "123213-123213123-sdfaf-123243" },
	    {programName: "Victims Fund", id: "123213-123213123-sdfaf-123217" },
	    {programName: "Victims Fund", id: "123213-123213123-sdfaf-123290" }
	],
	actions: {
		createFR() {
		  let { crmProgramId } = this.getProperties('crmProgramId');

		  var fundingRequest = FundingRequest.create({
		  	userId: this.get('session').currentUser.Email,
		  	crmProgramId: crmProgramId,		  	
		  });
		  var $this = this;
		  fundingRequest.createFR().then(function(value) {
		  	console.log("current user email is:" + fundingRequest.userId);
		  	console.log(value);		  
		  }, function(reason) {
		  	console.log(reason);
		  });
		  
		},
		testMethod() {
			console.log("hi there");
		}
	}
});

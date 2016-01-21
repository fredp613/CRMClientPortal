import Ember from 'ember';

export default Ember.Controller.extend({
	needs: ['fundingrequest'],
	actions: {
		testing() {
			this.get('controllers.fundingrequest').send('createFR', 'asdfasdfasdasdfasdfasdf')
			;
		}
	}
	// programs: [
	//     {programName: "Victims Fund", id: "123213-123213123-sdfaf-123213" },
	//     {programName: "Victims Fund", id: "123213-123213123-sdfaf-123243" },
	//     {programName: "Victims Fund", id: "123213-123213123-sdfaf-123217" },
	//     {programName: "Victims Fund", id: "123213-123213123-sdfaf-123290" }
	// ],
});

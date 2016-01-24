import DS from 'ember-data';

export default DS.Model.extend({
	name: DS.attr('string'),
  	startdate:  DS.attr('datetime'),
  	enddate: DS.attr('datetime'),
  	webdescription: DS.attr('string')  
});

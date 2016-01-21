export function initialize(application) {
 // application.store = container.lookup("service:store");
  //application.register('store:main', application.Store);
  //application.container.lookup('service:store')
  window.App = application;  // or window.Whatever
}

export default {
  name: 'global',
  initialize: initialize
};
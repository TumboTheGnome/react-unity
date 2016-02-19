#React Unity

As the name would suggest this plugin was inspired by Facebook's _React_ framework. _React Unity_ facilitates the creation of modular UI with data binding and path based contextual rendering. I'd like to note that this project is an in-progress experiment and should be used as such.

##How To
_React Unity_ is based around the idea of routers, paths, and elements. 

###Routers
Routers provide the logic behind a portion of UI. When the game starts all GameObjects with the _UIRouter_ component recursively evaluate their transform tree. Children with an existing _UIElement_ component are cached and indexed with their display flags. Should a child transform be found lacking the _UIElement_ component a copy will be added and the display flags from its parent element duplicated. The item will then be cached and indexed like normal. 

If in the process of caching a child transform is found to be an instance of _UIRouter_  that transform will be cached like normal but none of its child elements will be cached or indexed. 

Once everything has been cached and indexed each router without a parent router (i.e. lacking parent transform with the _UIRouter_ component) applies its defined default route. 

####Changing the Route
A new path can be applied to a router by passing it to the target _UIRouter's_  _UIRouter.Route_ property. Doing so will trigger the router to apply the flags included in the route and run a digest cycle.
 
    UIRouter Router = GetComponent<UIRouter>();
    Router.Route = "/root/article";

####Digest
Calling the Digest method on a router will prompt it to refresh the data binds on all contained elements. 

    Router.Digest();

###Paths
Paths are similar in structure to URL's or filesystem paths, though instead of referring to directories each segment references a display flag. When a path is applied to a router each of the flags are evaluated sequentially with each of the _UIElements_ associated with a given flag being toggled for display. All elements not flagged for display are hidden. Should an element associated with a flag be a router the reminder of the current route will be passed into the router for handling.

####Special Flags
Special flags are an experimental feature for imbedding programatic functionality into Paths. Currently only the _{current}_ flag is supported on the button control, though more are planned along with a generalization of the functionality.

#####{current}
Appends the current path in place of the flag.

###UIElement
_UIElement_ is the base for interfacing with the logic provided by _UIRouter_; simply create a new class and inherit from _UIElement_ and override the Show, Hide, and Refresh methods with your own logic. 

####Show
The _Show_ method is triggered whenever a flag associated with an element is included in the current route. Use this method to enable renderers, change materials, or apply whatever logic is needed for showing the active state of the element. A base implementation is provided which enables all components on the GameObject. 

    public override void Show(){
        renderer.enabled = true;
    }


####Hide
_Hide_ is triggered whenever a new route is passed to an elements parent router. Use this method to disable renderers, colliders, or apply any further logic for deactivating the element. DO NOT deactivate the element as this will prevent it from receiving future updates. A base implementation is provided which disables all components on the GameObject.

    public override void Hide(){
        renderer.enabled = false;
    }

####Refresh
Whenever a routers _UIRouter.Digest_ method is called _UIElement.Refresh_ is called on all contained elements. Data binding should handled in this method to reduce the need to reprocess Routes for updating data values. 

    public override void Refresh(){
       textComponent.text = obj.name;
    }
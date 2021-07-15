import './App.css';
import {Route} from "react-router-dom";
import EventsContainer from "./components/Events/EventsContainer";
import EventAreasContainer from "./components/EventAreas/EventAreasContainer";
import HeaderContainer from "./components/Header/HeaderContainer";
import Login from "./components/Login/Login";
import Register from "./components/Register/Register";
import BasketContainer from "./components/Basket/BasketContainer";
import PurchaseHistoryContainer from "./components/PurchaseHistory/PurchaseHistoryContainer";
import ProfileContainer from "./components/Profile/ProfileContainer";
import EventManagerAreaContainer from "./components/Areas/EventManagerArea/EventManagerAreaContainer";
import CreateEventContainer from "./components/Areas/EventManagerArea/Forms/CreateEventContainer";
import UpdateEventContainer from "./components/Areas/EventManagerArea/Forms/UpdateEventContainer";
import {Redirect} from "react-router";
import { routes } from "./components/Constants/routes";

const App = () => {
    return (
        <div className='app-wrapper'>
            <header className='header'><HeaderContainer/></header>
            <div className='content'>
                <Route path= {routes.register.href}
                       render={() => <Register/>}/>
                <Route path={routes.login.href}
                       render={() => <Login/>}/>
                <Route path={routes.eventArea.href}
                       render={() => <EventAreasContainer/>}/>
                <Route path={routes.home.href}
                       render={() => <EventsContainer/>}/>
                <Route path={routes.basket.href}
                       render={() => <BasketContainer/>}/>
                <Route path={routes.purchaseHistory.href}
                       render={() => <PurchaseHistoryContainer/>}/>
                <Route path={routes.profile.href}
                       render={() => <ProfileContainer/>}/>
                <Route path={routes.eventManagerArea.href}
                       render={() => <EventManagerAreaContainer/>}/>
                <Route path={routes.createEvent.href}
                       render={() => <CreateEventContainer/>}/>
                <Route path={routes.updateEvent.href}
                       render={() => <UpdateEventContainer/>}/>
                <Route exact path="/"
                       render={() => {
                           return (<Redirect to={routes.home.href}/>)
                       }}
                />
            </div>
        </div>
    );
}


export default App;

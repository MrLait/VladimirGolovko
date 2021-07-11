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

const App = () => {
    return (
        <div className='app-wrapper'>
            <header className= 'header'><HeaderContainer/></header>
            <div className='content'>
                <Route path='/register'
                       render={() => <Register/>}/>
                <Route path='/login'
                       render={() => <Login/>}/>
                <Route path='/eventArea/:eventId'
                       render={() => <EventAreasContainer/>}/>
                <Route path='/home'
                       render={() => <EventsContainer/>}/>
                <Route path='/basket'
                       render={() => <BasketContainer/>}/>
                <Route path='/purchaseHistory'
                       render={() => <PurchaseHistoryContainer/>}/>
                <Route path='/profile'
                       render={() => <ProfileContainer/>}/>
                <Route path='/eventManagerArea'
                       render={() => <EventManagerAreaContainer/>}/>
                <Route path='/create/event'
                       render={() => <CreateEventContainer/>}/>
            </div>
        </div>
    );
}


export default App;

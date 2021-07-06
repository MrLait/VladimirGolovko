import './App.css';
import {Route} from "react-router-dom";
import EventsContainer from "./components/Events/EventsContainer";
import EventAreasContainer from "./components/EventAreas/EventAreasContainer";
import HeaderContainer from "./components/Header/HeaderContainer";
import Login from "./components/Login/Login";

const App = () => {
    return (
            <div className='app-wrapper'>
                    <HeaderContainer />
                <div className='app-wrapper-content'>
                    <Route path='/home' render={() => <EventsContainer/>}/>
                    <Route path='/eventArea/:eventId' render={() => <EventAreasContainer/>}/>
                    <Route path='/login' render={() => <Login/>}/>
                </div>
            </div>
    );
}


export default App;

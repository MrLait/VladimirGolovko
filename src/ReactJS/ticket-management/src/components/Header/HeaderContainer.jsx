import React from "react";
import Header from "./Header";
import * as axios from "axios";
import {connect} from "react-redux";
import {setAuthData, isTokenValid} from "../../redux/auth-reducer";

class HeaderContainer extends React.Component {
    componentDidMount() {
        axios.post("https://localhost:5004/AccountUser/login",
            {
                "email": "eventManager@gmail.com",
                "password": "_Aa123456",
                "rememberMe": true
            })
            .then(response => {
                if (response.data) {
                    let token = response.data;
                    this.props.setAuthData(token);

                    axios.get("https://localhost:5004/AccountUser?token=" + token)
                        .then(response => {
                                if (response.state = "200"){
                                    this.props.isTokenValid(true);
                                }
                        });
                }
            });
    }

    render() {
        return <Header {...this.props}/>
    }
}

let mapStateToProps = (state) => ({
    isAuth: state.authPage.isAuth,
    token: state.authPage.token,

});

export default connect(mapStateToProps, {setAuthData, isTokenValid })(HeaderContainer);
import React from "react";

const TimeZoneOffSetSelector = (props) => {
    const onChange = (currProps) => {
        props.editTimeZoneOffset(currProps.target.value);
    }
    return  <select id={props.timeZoneOffSet} name={'timeZoneOffset'} onChange={onChange} value={props.timeZoneOffSet}>
        <option></option>
        <option value="-12">(GMT-12:00) International Date Line West</option>
        <option value="-11">(GMT-11:00) Midway Island, Samoa</option>
        <option value="-10">(GMT-10:00) Hawaii</option>
        <option value="-9">(GMT-09:00) Alaska</option>
        <option value="-8">(GMT-08:00) Tijuana, Baja California</option>
        <option value="-7">(GMT-07:00) Mountain Time (US & Canada)</option>
        <option value="-6">(GMT-06:00) Central America</option>
        <option value="-5">(GMT-05:00) Bogota, Lima, Quito, Rio Branco</option>
        <option value="-4">(GMT-04:00) Atlantic Time (Canada)</option>
        <option value="-3.5">(GMT-03:30) Newfoundland</option>
        <option value="-3">(GMT-03:00) Montevideo</option>
        <option value="-2">(GMT-02:00) Mid-Atlantic</option>
        <option value="-1">(GMT-01:00) Azores</option>
        <option value="0">(GMT+00:00) Greenwich Mean Time : Dublin, Edinburgh, Lisbon, London</option>
        <option value="1">(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna</option>
        <option value="2">(GMT+02:00) Amman</option>
        <option value="2">(GMT+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius</option>
        <option value="3">(GMT+03:00) Minsk, Moscow, St. Petersburg, Volgograd</option>
        <option value="3.5">(GMT+03:30) Tehran</option>
        <option value="4">(GMT+04:00) Yerevan</option>
        <option value="4.5">(GMT+04:30) Kabul</option>
        <option value="5">(GMT+05:00) Islamabad, Karachi, Tashkent</option>
        <option value="5.5">(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi</option>
        <option value="5.75">(GMT+05:45) Kathmandu</option>
        <option value="6">(GMT+06:00) Astana, Dhaka</option>
        <option value="6.5">(GMT+06:30) Yangon (Rangoon)</option>
        <option value="7">(GMT+07:00) Krasnoyarsk</option>
        <option value="8">(GMT+08:00) Taipei</option>
        <option value="9">(GMT+09:00) Yakutsk</option>
        <option value="9.5">(GMT+09:30) Darwin</option>
        <option value="10">(GMT+10:00) Vladivostok</option>
        <option value="11">(GMT+11:00) Magadan, Solomon Is., New Caledonia</option>
        <option value="12">(GMT+12:00) Fiji, Kamchatka, Marshall Is.</option>
        <option value="13">(GMT+13:00) Nuku'alofa</option>
    </select>
}

export default TimeZoneOffSetSelector;
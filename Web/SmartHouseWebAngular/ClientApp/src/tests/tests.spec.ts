import { GoalServiceTest } from "./services/http.goal.test";
import { LoggerServiceTest } from "./services/http.logger.test";
import { WeatherServiceTest } from "./services/http.weather.test";
import { AppComponentTest } from "./components/app.component.test";
import { GoalComponentTest } from "./components/goal.component.test";
import { WeatherComponentTest } from "./components/weather.component.test";
import { LoggerComponentTest } from "./components/logger.component.test";


describe('All tests', () => {

    // Test service.
    GoalServiceTest.test();
    LoggerServiceTest.test();
    WeatherServiceTest.test();

    // Test components.
    AppComponentTest.test();
    GoalComponentTest.test();
    LoggerComponentTest.test();
    WeatherComponentTest.test();
});



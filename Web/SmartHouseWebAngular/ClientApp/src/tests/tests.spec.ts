
import { AppComponentTest, GoalComponentTest, LoggerComponentTest, WeatherComponentTest } from './components';
import { GoalServiceTest, LoggerServiceTest, WeatherServiceTest } from './services';

describe('Service tests', () => {
    
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



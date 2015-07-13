# If This Then That Poster
Small command line utility to help posting triggers to If This Then That

Usage: 
The utility can be used via command line arguments or files

When running the utility with no arguments, the trigger parameters are read from the data.txt file
When running with arguments the contents of the data.txt file can be passed in as a string for the first argument

The format of the data.txt file is
{
  trigger: 'trigger name',
  data: {
    name1: 'value 2'
  }
}

Additional parameters for the trigger are stored in the [trigger name].txt file

The trigger config file has the following format:

{
  method: '[POST|GET|DELETE|PATCH|HEAD|..etc]',
  mime: '[mime type e.g. application/json]'
}

Global settings are found in the config.txt file with the format:

{
  url: 'the url of the server, with {0} = trigger, {1} = key',
  key: 'your ittt API key'
}
